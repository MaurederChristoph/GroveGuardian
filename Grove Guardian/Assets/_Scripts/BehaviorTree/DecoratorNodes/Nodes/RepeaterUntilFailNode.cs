public class RepeaterUntilFailNode : DecoratorNode {
    public RepeaterUntilFailNode(Node child) {
        _child = child;
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Execute() {
        _child.Enter();
        _child.Execute();
    }

    protected override void ChildFinish(ReturnState state) {
        if(state == ReturnState.failure) {
            OnFinish.Invoke(ReturnState.success);
            Exit();
            return;
        }
        Execute();
    }
}
