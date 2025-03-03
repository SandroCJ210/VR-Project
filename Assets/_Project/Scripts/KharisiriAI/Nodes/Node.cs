using System.Collections.Generic;

public abstract class Node
{
    public abstract State Evaluate();
    protected List<Node> _children;

    public void AddChild(Node child) => _children.Add(child);
}

