using System.Threading.Tasks;
using UnityEngine;
public class Root : Node {
    private readonly Node _child;

    public Root(Node child) {
        _child = child;
    }

    public override void Enter() {
        _child.Enter();
        _child.OnFinish += ChildFinish;
        Execute();
    }

    public override void Execute() {
        _child.Execute();
    }

    public override void Exit() {
        _child.OnFinish -= ChildFinish;
    }

    public override void Interrupt() {
        _child.Interrupt();
        Exit();
        Enter();
    }

    protected override void ChildFinish(ReturnState _) {
        Exit();
        Enter();
    }
}
