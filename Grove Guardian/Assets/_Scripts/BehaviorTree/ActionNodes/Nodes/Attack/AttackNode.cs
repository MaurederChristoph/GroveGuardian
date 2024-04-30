using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AttackNode : ActionNode {
    private int _damage;

    public AttackNode(EnemyBrain enemyBrain, int Dmg) {
        EnemyBrain = enemyBrain;
        _damage = Dmg;
    }

    public override void Execute() {
        EnemyBrain.Attack(OnFinish, _damage);
    }

    public override void Interrupt() {
        EnemyBrain.OnInterrupt?.Invoke();
    }
}
