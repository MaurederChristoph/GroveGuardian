using System.Collections.Generic;
using System.Threading.Tasks;

public class ParallelWaitNode : CompositeNode {
    private int _childrenFinished = 0;
    public ParallelWaitNode(params Node[] children) {
        _children = new List<Node>(children);
    }

    public override void Enter() {
        base.Enter();
        foreach(var child in _children) {
            child.Enter();
        }
    }

    public override void Execute() {
        foreach(var child in _children) {
            child.Execute();
        }
    }

    protected override void ChildFinish(ReturnState state) {
        _childrenFinished++;
        if(_childrenFinished >= _children.Count) {
            OnFinish.Invoke(state);
            Exit();
            return;
        }
        foreach(var child in _children) {
            if(!child.isRunning) {
                child.Execute();
            }
        }
    }
}
