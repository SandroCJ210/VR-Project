using UnityEngine;

public class Kharisiri : MonoBehaviour
{
    [SerializeField] private KharisiriController _controller;

    public void StartBehaviorTree()
    {
        _controller.StartBehaviorTree();
    }
}
