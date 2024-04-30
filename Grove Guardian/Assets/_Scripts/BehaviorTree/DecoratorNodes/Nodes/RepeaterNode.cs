public class RepeaterNode : DecoratorNode {
    private readonly int _repeatAmount;
    private int _repeated = 0;
    public RepeaterNode(Node child, int repeatAmount) {
        _child = child;
        _repeatAmount = repeatAmount;
    }

    public override void Enter() {
        base.Enter();
        _repeated = 0;
    }

    public override void Execute() {
        _child.Enter();
        _child.Execute();
    }

    protected override void ChildFinish(ReturnState state) {
        if(state == ReturnState.failure) {
            OnFinish.Invoke(state);
            Exit();
            return;
        }
        if(_repeatAmount <= ++_repeated) {
            OnFinish.Invoke(ReturnState.success);
            Exit();
            return;
        }
        Execute();
    }
}
