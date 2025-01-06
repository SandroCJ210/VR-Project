using System;

public abstract class Node
{
    public enum State { Running, Success, Failure }
    public string nodeName;
    public Type nodeType;
    public abstract State Evaluate();
}

