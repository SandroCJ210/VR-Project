using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KharisiriController : MonoBehaviour
{
    KharisiriBrain _brain;
    KharisiriTree _behaviorTree;
    NavMeshAgent _agent;

    Transform[] _patrolPoints;

    void Start()
    {
        ConfiguratePath();
        SetBrain();
        _behaviorTree = new("Selector", _brain);
    }

    void ConfiguratePath(){
        _agent = GetComponent<NavMeshAgent>();

        GameObject patrolPointsParent = GameObject.Find("PatrolPoints");

        if (patrolPointsParent != null)
        {
            _patrolPoints = new Transform[patrolPointsParent.transform.childCount];
            for (int i = 0; i < patrolPointsParent.transform.childCount; i++)
            {
                _patrolPoints[i] = patrolPointsParent.transform.GetChild(i);
            }
        }
    }

    void SetBrain(){
        _brain = new KharisiriBrain();
        _brain.SetData("DetectionGauge", 0f);
        _brain.SetData("CanSeePlayer", false);
        _brain.SetData("HasValidRoute", false);
        _brain.SetData("HasSeenPlayer", false);
        _brain.SetData("PlayerDetected", false);

        Queue<int> patrolRoute = new();
        patrolRoute.Enqueue(1);
        patrolRoute.Enqueue(2);
        patrolRoute.Enqueue(3);
        _brain.SetData("PatrolRoute", patrolRoute);
        _brain.SetData("PlayerLastSeenPosition", new Vector3());
        _brain.SetData("HardInvestigateArea", new KeyValuePair<Vector3, float>(new Vector3(), 0f));
        _brain.SetData("HardInvestigateRoom", 0f);
        _brain.SetData("SuspectAreaDelayTime", 0f);
        _brain.SetData("Motivation", Motivation.Patrol);
    }

    void Update()
    {
        _behaviorTree.Evaluate();
    }
}
