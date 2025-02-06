using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KharisiriTree
{
    Node _rootNode;
    KharisiriBrain _brain;

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
        ActionNode defineStartRoute = new(() =>
        {
            var patrolRoute = _brain.GetData<Queue<int>>("PatrolRoute");
            if (patrolRoute.Count == 3)
            {
                return State.Success;
            }

            while (patrolRoute.Count < 3)
            {
                int nextRoom = GetNextRoom();
                patrolRoute.Enqueue(nextRoom);
            }
            Debug.Log("Patrol Route Defined");
            Debug.Log("Patrol Route: " + string.Join(", ", patrolRoute));
            _brain.SetData("PatrolRoute", patrolRoute);
            return State.Success;
        });
        ActionNode investigateArea = new(() =>
        {
            var navMeshAgent = _brain.GetData<NavMeshAgent>("NavMeshAgent");
            int nextNode = _brain.GetData<Queue<int>>("PatrolRoute").Peek();
            Transform[] patrolPoints = _brain.GetData<Transform[]>("PatrolPoints");
            navMeshAgent.SetDestination(patrolPoints[nextNode].position);
            
            if(Vector3.Distance(navMeshAgent.transform.position, patrolPoints[nextNode].position) < 1f)
            {
                _brain.GetData<Queue<int>>("PatrolRoute").Dequeue();
                return State.Success;
            }
            return State.Running;
        });
        ActionNode getNextRoom = new(() =>
        {
            var patrolRoute = _brain.GetData<Queue<int>>("PatrolRoute");
            if (patrolRoute.Count == 3)
            {
                return State.Success;
            }

            while (patrolRoute.Count < 3)
            {
                int nextRoom = GetNextRoom();
                patrolRoute.Enqueue(nextRoom);
            }
            Debug.Log("Patrol Route Defined");
            Debug.Log("Patrol Route: " + string.Join(", ", patrolRoute));
            _brain.SetData("PatrolRoute", patrolRoute);
            return State.Success;
        });
        sequence.AddChild(defineStartRoute);
        sequence.AddChild(investigateArea);
        sequence.AddChild(getNextRoom);
        return new ConditionalNode(() =>
            _brain.GetData<Motivation>("CurrentMotivation") == Motivation.Patrol
        , sequence);
    }

    int GetNextRoom()
    {
        int actualRoom = _brain.GetData<int>("CurrentRoom");
        int actualNode = _brain.GetData<int>("CurrentPatrolPoint");
        bool isInRoom = _brain.GetData<bool>("IsInRoom");
        Transform[] patrolPoints = _brain.GetData<Transform[]>("PatrolPoints");
        Dictionary<int, List<int>> roomPatrolPoints = _brain.GetData<Dictionary<int, List<int>>>("RoomPatrolPoints");
        int childsNodes = roomPatrolPoints[actualRoom].Count;
        if (childsNodes == 1)
        {
            actualRoom++;
            actualNode = roomPatrolPoints[actualRoom][0];
            isInRoom = false;
        }
        else
        {
            if (isInRoom)
            {
                actualRoom++;
                actualNode = roomPatrolPoints[actualRoom][0];
                isInRoom = false;
            }
            else
            {
                actualNode++;
                isInRoom = true;
            }
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