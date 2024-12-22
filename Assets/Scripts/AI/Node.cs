using System.Collections.Generic;

public abstract class Node
{
    public enum State { Running, Success, Failure }
    public abstract State Evaluate();
}

public class Selector : Node
{
    private List<Node> _children = new();

    public Selector(List<Node> children)
    {
        _children = children;
    }

    public override State Evaluate()
    {
        foreach (var child in _children)
        {
            State result = child.Evaluate();
            if (result == State.Success)
                return State.Success;
            if (result == State.Running)
                return State.Running;
        }
        return State.Failure;
    }
}

public class Sequence : Node
{
    private List<Node> _children = new();

    public Sequence(List<Node> children)
    {
        _children = children;
    }

    public override State Evaluate()
    {
        foreach (var child in _children)
        {
            State result = child.Evaluate();
            if (result == State.Failure)
                return State.Failure;
            if (result == State.Running)
                return State.Running;
        }
        return State.Success;
    }
}

public class ActionNode : Node
{
    private System.Func<State> _action;

    public ActionNode(System.Func<State> action)
    {
        _action = action;
    }

    public override State Evaluate()
    {
        return _action();
    }
}
