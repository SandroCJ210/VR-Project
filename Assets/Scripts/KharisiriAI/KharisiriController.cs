using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KharisiriController : MonoBehaviour
{
    KharisiriBrain _brain;
    KharisiriTree _behaviorTree;
    NavMeshAgent _agent;

    Dictionary<int, List<int>> _roomPatrolPoints;
    Transform[] _patrolPoints;

    void Start()
    {
        ConfiguratePath();
        SetBrain();
        _behaviorTree = new("Selector", _brain);
        StartCoroutine(RunBehaviorTree());
    }

    IEnumerator RunBehaviorTree()
    {
        while (true)
        {
            _behaviorTree.Evaluate();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void ConfiguratePath()
    {
        _agent = GetComponent<NavMeshAgent>();

        GameObject patrolPointsParent = GameObject.Find("PatrolNodes");

        if (patrolPointsParent != null)
        {
            _patrolPoints = new Transform[patrolPointsParent.transform.childCount];
            _roomPatrolPoints = new Dictionary<int, List<int>>();
            for (int i = 0; i < patrolPointsParent.transform.childCount; i++)
            {
                _patrolPoints[i] = patrolPointsParent.transform.GetChild(i);
                int idRoom = _patrolPoints[i].GetComponent<PathNode>()._roomId;
                if (_roomPatrolPoints.ContainsKey(idRoom))
                {
                    _roomPatrolPoints[idRoom].Add(i);
                }
                else
                {
                    _roomPatrolPoints.Add(idRoom, new List<int> { i });
                }
            }
        }
    }

    void SetBrain()
    {
        _brain = new KharisiriBrain();
        _brain.SetData("DetectionGauge", 0f);
        _brain.SetData("CanSeePlayer", false);
        _brain.SetData("HasValidRoute", false);
        _brain.SetData("HasSeenPlayer", false);
        _brain.SetData("PlayerDetected", false);

        Queue<int> patrolRoute = new();
        _brain.SetData("PatrolRoute", patrolRoute);
        _brain.SetData("CurrentPatrolPoint", 0);
        _brain.SetData("CurrentRoom", 0);
        _brain.SetData("IsInRoom", false);
        _brain.SetData("PatrolPoints", _patrolPoints);
        _brain.SetData("RoomPatrolPoints", _roomPatrolPoints);
        _brain.SetData("PlayerLastSeenPosition", new Vector3());
        _brain.SetData("HardInvestigateArea", new KeyValuePair<Vector3, float>(new Vector3(), 0f));
        _brain.SetData("HardInvestigateRoom", 0f);
        _brain.SetData("SuspectAreaDelayTime", 0f);
        _brain.SetData("Motivation", Motivation.Patrol);
        _brain.SetData("NavMeshAgent", _agent);
    }
}
