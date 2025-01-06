public class RootNode : Node{
    public Node childNode;
    public RootNode()
    {
        nodeName = "Root";
        nodeType = typeof(RootNode);
        childNode = new SelectorNode();
    }
    public override State Evaluate()
    {
        return childNode.Evaluate();
    }
}