using System;

public abstract class ActionNode : Node {
    public Action OnFinishExecution;
    protected EnemyBrain EnemyBrain;
    protected override void ChildFinish(ReturnState state) { }
    public override void Enter() {
        isRunning = true;
    }
    public override void Exit() {
        isRunning = false;
    }
}
