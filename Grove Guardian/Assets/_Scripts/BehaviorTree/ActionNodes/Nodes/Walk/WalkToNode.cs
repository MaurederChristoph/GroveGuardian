using UnityEngine;

public class WalkToNode : ActionNode {
    private readonly Destination _destination;
    public WalkToNode(EnemyBrain enemyBrain, Destination destination) {
        EnemyBrain = enemyBrain;
        _destination = destination;
    }

    public override void Execute() {
        EnemyBrain.StartWalkingTo(OnFinish, _destination);
    }

    public override void Interrupt() {
        EnemyBrain.OnInterrupt?.Invoke();
    }
}

public enum Destination { 
    Player,
    Tree,
    Mold
}
