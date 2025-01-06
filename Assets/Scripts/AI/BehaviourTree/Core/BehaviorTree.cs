using UnityEngine;

[CreateAssetMenu(fileName = "NewBehaviorTree", menuName = "MeowAI/BehaviorTree")]
public class BehaviorTree : ScriptableObject
{
    [SerializeField] private RootNode _rootNode;
    public string treeName = "Behavior Tree";
    public RootNode GetRootNode() => _rootNode;
    private void OnEnable()
    {
        if (_rootNode == null)
        {
            _rootNode = new RootNode();
        }
    }
    public void Evaluate()
    {
        _rootNode.Evaluate();
    }
}
