using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchNode : ActionNode {
    readonly float _searchTime;
    public SearchNode(EnemyBrain enemyBrain, float searchTime) {
        _searchTime = searchTime;
        EnemyBrain = enemyBrain;
    }

    public override void Execute() {
        EnemyBrain.SearchForPlayer(OnFinish, _searchTime);
    }

    public override void Interrupt() {
        EnemyBrain.OnInterrupt?.Invoke();
    }
}
