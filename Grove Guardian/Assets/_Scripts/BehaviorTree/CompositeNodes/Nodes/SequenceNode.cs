using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SequenceNode : CompositeNode {
    private int _currentChildIndex;

    public SequenceNode(params Node[] children) {
        _children = new List<Node>(children);
    }

    public override void Enter() {
        base.Enter();
        _currentChildIndex = 0;
    }

    public override void Execute() {
        _currentChild = _children[_currentChildIndex];
        _children[_currentChildIndex].Enter();
        _children[_currentChildIndex].Execute();
    }

    protected override void ChildFinish(ReturnState state) {
        _children[_currentChildIndex].Exit();
        if (state == ReturnState.failure) {
            OnFinish.Invoke(ReturnState.failure);
            Exit();
            return;
        }

        _currentChildIndex++;
        if (_currentChildIndex >= _children.Count) {
            OnFinish.Invoke(ReturnState.success);
            Exit();
        } else {
            Execute();
        }
    }
}
