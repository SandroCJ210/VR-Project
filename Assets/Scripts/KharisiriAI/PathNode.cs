using UnityEngine;

public class PathNode : MonoBehaviour
{
    [SerializeField] private PathNode[] _neighbors;
    [SerializeField] private int _nodeIndex;
}
