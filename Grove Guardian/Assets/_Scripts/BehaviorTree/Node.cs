using System;

public abstract class Node {
    protected bool _init = true;
    public bool isRunning = false;
    public Action<ReturnState> OnFinish;
    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
    protected abstract void ChildFinish(ReturnState state);
    public abstract void Interrupt();
}

public enum ReturnState {
    success,
    failure
}
