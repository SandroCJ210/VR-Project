using System;

public class ActionNode : Node
{
    private Func<State> _action;

    public ActionNode(Func<State> action)
    {
        _action = action;
    }

    public void SetAction(Func<State> action)
    {
        _action = action;
    }

    public override State Evaluate()
    {
        return _action();
    }
}