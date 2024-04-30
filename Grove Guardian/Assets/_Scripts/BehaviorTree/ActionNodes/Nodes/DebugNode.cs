using UnityEngine;

public class DebugNode : ActionNode {
    private readonly string _message;
    public DebugNode(string message) {
        _message = message;
    }

    public override void Execute() {
        Debug.Log(_message);
        OnFinish.Invoke(ReturnState.success);
    }

    public override void Interrupt() { }
}
