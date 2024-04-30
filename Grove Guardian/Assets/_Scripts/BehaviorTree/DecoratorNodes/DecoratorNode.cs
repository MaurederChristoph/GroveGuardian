public abstract class DecoratorNode : Node {
    protected Node _child;

    public override void Interrupt() {
        _child.Interrupt();
    }
    public override void Enter() {
        isRunning = true;
        if(_init) {
            _init = false;
            _child.OnFinish += ChildFinish;
        }
    }

    public override void Exit() {
        _init = false;
    }
}
