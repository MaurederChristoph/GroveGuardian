using UnityEngine;

public class BlackboardCheck : ActionNode {
    readonly Blackboard _blackboard;
    readonly string _check;
    public BlackboardCheck(Blackboard blackboard, string check) {
        _blackboard = blackboard;
        _check = check;
    }
    public override void Execute() {
        if(_blackboard.Check(_check)) {
            OnFinish.Invoke(ReturnState.success);
        } else {
            OnFinish.Invoke(ReturnState.failure);
        }
    }

    public override void Interrupt() { }
}
