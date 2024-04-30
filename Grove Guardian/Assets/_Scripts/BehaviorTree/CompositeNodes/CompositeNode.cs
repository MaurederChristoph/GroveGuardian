using System.Collections.Generic;

public abstract class CompositeNode : Node {
    protected List<Node> _children;
    protected Node _currentChild;

    public override void Interrupt() {
        _currentChild.Interrupt();
    }
    public override void Enter() {
        isRunning = true;
        if(_init) {
            _init = false;
            foreach(var child in _children) {
                child.OnFinish += ChildFinish;
            }
        }
    }

    public override void Exit() {
        _init = false;
    }
}
