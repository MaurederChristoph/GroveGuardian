using System.Collections.Generic;

public class SelectorNode : CompositeNode {
    private int _currentChildIndex;

    public SelectorNode(params Node[] children) {
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
        if(state == ReturnState.success) {
            OnFinish.Invoke(ReturnState.success);
            Exit();
            return;
        }
        _currentChildIndex++;
        if(_currentChildIndex >= _children.Count) {
            OnFinish.Invoke(ReturnState.failure);
            Exit();
        } else {
            Execute();
        }
    }
}
