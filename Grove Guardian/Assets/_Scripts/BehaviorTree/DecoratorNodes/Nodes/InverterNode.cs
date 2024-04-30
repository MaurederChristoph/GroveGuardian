public class InverterNode : DecoratorNode {
    public InverterNode(Node child) {
        _child = child;
    }

    public override void Enter() {
        base.Enter();
        _child.Enter();
    }


    public override void Execute() {
        _child.Execute();
    }
    protected override void ChildFinish(ReturnState state) {
        if(state == ReturnState.failure) {
            OnFinish.Invoke(ReturnState.success);
        }
        if(state == ReturnState.success) {
            OnFinish.Invoke(ReturnState.failure);
        }
        Exit();
    }
}