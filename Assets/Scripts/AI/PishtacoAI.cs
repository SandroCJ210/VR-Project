using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class PishtacoAI : MonoBehaviour
{
    private Node _behaviorTree;
    private NavMeshAgent _agent;
    private Transform _player;
    private Vector3 _lastKnownPosition;
    private float _detectionRadius = 10f;
    private bool _playerVisible = false;

    private Transform[] _patrolPoints;
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _chaseSpeed = 5f;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _behaviorTree = CreateBehaviorTree();

        GameObject patrolPointsParent = GameObject.Find("PatrolPoints");

        if (patrolPointsParent != null)
        {
            _patrolPoints = new Transform[patrolPointsParent.transform.childCount];
            for (int i = 0; i < patrolPointsParent.transform.childCount; i++)
            {
                _patrolPoints[i] = patrolPointsParent.transform.GetChild(i);
            }
        }
        _agent.speed = _patrolSpeed;
    }

    void Update()
    {
        _behaviorTree.Evaluate();
    }

    private Node CreateBehaviorTree()
    {
        var detectPlayer = new ActionNode(DetectPlayer);
        var chasePlayer = new ActionNode(ChasePlayer);
        var searchLastKnownPosition = new ActionNode(SearchLastKnownPosition);
        var patrol = new ActionNode(Patrol);

        var alertSequence = new Sequence(new List<Node> { detectPlayer, chasePlayer });
        var searchSequence = new Sequence(new List<Node> { searchLastKnownPosition });

        return new Selector(new List<Node>
        {
            alertSequence,
            searchSequence,
            patrol
        });
    }

    private Node.State DetectPlayer()
    {
        if (Vector3.Distance(transform.position, _player.position) < _detectionRadius)
        {
            Ray ray = new(transform.position, (_player.position - transform.position).normalized);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Player"))
            {
                _playerVisible = true;
                _lastKnownPosition = _player.position;
                return Node.State.Success;
            }
        }
        _playerVisible = false;
        return Node.State.Failure;
    }

    private Node.State ChasePlayer()
    {
        if (_playerVisible)
        {
            _agent.speed = _chaseSpeed;
            _agent.SetDestination(_player.position);
            return Node.State.Running;
        }
        return Node.State.Failure;
    }

    private Node.State SearchLastKnownPosition()
    {
        if (!_playerVisible && _lastKnownPosition != Vector3.zero)
        {
            _agent.speed = _patrolSpeed;
            _agent.SetDestination(_lastKnownPosition);
            if (Vector3.Distance(transform.position, _lastKnownPosition) < 1f)
            {
                _lastKnownPosition = Vector3.zero;
                return Node.State.Success;
            }
            return Node.State.Running;
        }
        return Node.State.Failure;
    }

    private Node.State Patrol()
    {
        if(!_playerVisible && _lastKnownPosition == Vector3.zero)
        {
            if (_patrolPoints.Length == 0)
            {
                return Node.State.Failure;
            }

            _agent.speed = _patrolSpeed;
            if (_agent.remainingDistance < 0.5f)
            {
                _agent.SetDestination(_patrolPoints[Random.Range(0, _patrolPoints.Length)].position);
            }
        }
        return Node.State.Running;
    }
}
