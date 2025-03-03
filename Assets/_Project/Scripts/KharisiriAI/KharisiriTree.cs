using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KharisiriTree
{
    Node _rootNode;
    KharisiriBrain _brain;
    bool needsRandomRoom = false;

    public KharisiriTree(string nodeType, KharisiriBrain brain)
    {
        if (nodeType == "Selector")
        {
            _rootNode = new SelectorNode();
        }
        else if (nodeType == "Sequence")
        {
            _rootNode = new SequenceNode();
        }
        _brain = brain;
        _rootNode.AddChild(PatrolMotivation());
    }

    Node PatrolMotivation()
    {
        Node sequence = new SequenceNode();

        ActionNode calculateNextRoom = new(() =>
        {
            if (_brain.GetData<Queue<int>>("PatrolRoute").Count > 0) return State.Success;
            Debug.Log("Calculating Next Room...");
            int nextRoom = GetNextRoom();
            if (nextRoom == -1) return State.Failure;

            var patrolRoute = _brain.GetData<Queue<int>>("PatrolRoute");
            patrolRoute.Enqueue(nextRoom);
            _brain.SetData("PatrolRoute", patrolRoute);

            return State.Success;
        });

        Node investigateAreaSequence = new SequenceNode();

        ActionNode walkToNextRoom = new(() =>
        {
            Debug.Log("Walking to next room...");
            var navMeshAgent = _brain.GetData<NavMeshAgent>("NavMeshAgent");
            var patrolRoute = _brain.GetData<Queue<int>>("PatrolRoute");
            Transform[] patrolPoints = _brain.GetData<Transform[]>("PatrolPoints");

            if (patrolRoute.Count == 0)
                return State.Failure;

            int nextNode = patrolRoute.Peek();
            navMeshAgent.SetDestination(patrolPoints[nextNode].position);

            if (Vector3.Distance(navMeshAgent.transform.position, patrolPoints[nextNode].position) < 1f)
            {
                patrolRoute.Dequeue();
                _brain.SetData("PatrolRoute", patrolRoute);
                return State.Success;
            }
            return State.Running;
        });

        ActionNode checkForPlayer = new(() =>
        {
            if (_brain.GetData<bool>("CanSeePlayer"))
            {
                Debug.Log("Player detected! Changing behavior...");
                _brain.SetData("Motivation", Motivation.Investigate);
                return State.Success;
            }
            return State.Failure;
        });

        ActionNode getNextRoom = new(() =>
        {
            var patrolRoute = _brain.GetData<Queue<int>>("PatrolRoute");
            if (patrolRoute.Count == 0)
            {
                patrolRoute.Enqueue(GetNextRoom());
                _brain.SetData("PatrolRoute", patrolRoute);
            }
            return State.Success;
        });

        investigateAreaSequence.AddChild(walkToNextRoom);
        investigateAreaSequence.AddChild(checkForPlayer);

        sequence.AddChild(calculateNextRoom);
        sequence.AddChild(investigateAreaSequence);
        sequence.AddChild(getNextRoom);

        return new ConditionalNode(() =>
            _brain.GetData<Motivation>("Motivation") == Motivation.Patrol
        , sequence);
    }

    int GetNextRoom()
    {
        int actualRoom = _brain.GetData<int>("CurrentRoom");
        int actualNode = _brain.GetData<int>("CurrentPatrolPoint");
        bool isInRoom = _brain.GetData<bool>("IsInRoom");
        Dictionary<int, List<int>> roomPatrolPoints = _brain.GetData<Dictionary<int, List<int>>>("RoomPatrolPoints");

        int childsNodes = roomPatrolPoints[actualRoom].Count;
        if (childsNodes == 1 || isInRoom)
        {
            actualRoom = needsRandomRoom ? Random.Range(0, roomPatrolPoints.Count) : actualRoom + 1;
            Debug.Log("Getting next room " + actualRoom);

            actualNode = needsRandomRoom ? Random.Range(0, roomPatrolPoints[actualRoom].Count) : roomPatrolPoints[actualRoom][0];
            
            isInRoom = false;
            
            needsRandomRoom = actualRoom >= roomPatrolPoints.Count - 1;
        }
        else
        {
            actualNode++;
            isInRoom = true;
        }
        _brain.SetData("CurrentRoom", actualRoom);
        _brain.SetData("CurrentPatrolPoint", actualNode);
        _brain.SetData("IsInRoom", isInRoom);
        return actualNode;
    }

    public void Evaluate()
    {
        _rootNode.Evaluate();
    }
}
