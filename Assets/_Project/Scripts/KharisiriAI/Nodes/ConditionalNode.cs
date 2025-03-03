using System;

public class ConditionalNode : Node
{
    private Func<bool> _condition;
    private Node _child;

    public ConditionalNode(Func<bool> condition, Node child)
    {
        _condition = condition;
        _child = child;
    }

    public override State Evaluate()
    {
        if (_condition())
        {
            return _child.Evaluate();
        }
        return State.Failure;
    }
}
