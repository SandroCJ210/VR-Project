using System.Collections.Generic;

public class SequenceNode : Node
{
    public SequenceNode()
    {
        _children = new List<Node>();
    }
    public SequenceNode(List<Node> children)
    {
        _children = children;
    }

    public override State Evaluate()
    {
        foreach (var child in _children)
        {
            State result = child.Evaluate();
            if (result != State.Success) return result;
        }
        return State.Success;
    }
}