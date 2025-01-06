public class ActionNode : Node
{
    private System.Func<State> _action;

    public ActionNode()
    {
        nodeName = "Action Node";
        nodeType = typeof(ActionNode);
    }

    public ActionNode(System.Func<State> action)
    {
        _action = action;
    }

    public void SetAction(System.Func<State> action)
    {
        _action = action;
    }

    public override State Evaluate()
    {
        return _action();
    }
}