using System.Collections.Generic;

public class SequenceNode : Node
{
    private List<Node> _children = new();
    public SequenceNode() { 
        nodeName = "Sequence";
        nodeType = typeof(SequenceNode);
    }
    public void AddChild(Node child) => _children.Add(child);

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