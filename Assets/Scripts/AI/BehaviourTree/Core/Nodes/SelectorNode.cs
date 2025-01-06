using System.Collections.Generic;

public class SelectorNode : Node
{
    private List<Node> _children = new();

    public SelectorNode() { 
        nodeName = "Selector";
        nodeType = typeof(SelectorNode);
    }

    public void AddChild(Node child) => _children.Add(child);

    public SelectorNode(List<Node> children)
    {
        _children = children;
    }

    public override State Evaluate()
    {
        foreach (var child in _children)
        {
            State result = child.Evaluate();
            if (result != State.Failure) return result;
        }
        return State.Failure;
    }
}