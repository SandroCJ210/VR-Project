using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KharisiriTree
{
    Node _rootNode;
    KharisiriBrain _brain;
    Node GetRootNode() => _rootNode;

    public KharisiriTree(string nodeType, KharisiriBrain brain)
    {
        if (nodeType == "Selector")
        {
            _rootNode = new SelectorNode();
        }
        else if (nodeType == "Sequence")
        {
            _rootNode = new SequenceNode();
        }
        _brain = brain;
        _rootNode.AddChild(PatrolMotivation());
    }

    Node PatrolMotivation()
    {
        Node sequence = new SequenceNode();
        ActionNode DefineStartRoute = new(() =>
        {
            var patrolRoute = _brain.GetData<Queue<int>>("PatrolRoute");
            if (patrolRoute.Count == 3)
            {
                return State.Success;
            }

            while (patrolRoute.Count < 3)
            {
                // TODO - Get nearest patrol points
            }
            _brain.SetData("PatrolRoute", patrolRoute);
            return State.Success;
        });
        return sequence;
    }

    public void Evaluate()
    {
        _rootNode.Evaluate();
    }
}