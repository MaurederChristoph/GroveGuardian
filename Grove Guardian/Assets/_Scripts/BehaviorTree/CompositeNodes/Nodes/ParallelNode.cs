using System.Collections.Generic;

public class ParallelNode : CompositeNode {
    public ParallelNode(params Node[] children) {
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
        Interrupt();
        OnFinish.Invoke(state);
        Exit();
    }
}
