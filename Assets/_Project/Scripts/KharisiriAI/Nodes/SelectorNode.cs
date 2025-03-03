using System.Collections.Generic;

public class SelectorNode : Node
{
    public SelectorNode()
    {
        _children = new List<Node>();
    }

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