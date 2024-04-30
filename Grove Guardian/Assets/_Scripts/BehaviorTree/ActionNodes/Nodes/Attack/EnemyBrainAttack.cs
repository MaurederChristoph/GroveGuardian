using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyBrain {
    [SerializeField] private Blackboard _blackboard;
    private int _dmg;
    private Transform _target;

    public void Attack(Action<ReturnState> onFinish, int dmg) {
        _onFinish = onFinish;
        _dmg = dmg;
        if(_blackboard.PlayerInRange) {
            _target = _player;
            MakeAttack();
        } else if(_blackboard.TreeInRange) {
            _target = _tree;
            MakeAttack();
        }
        _animator.SetBool("isWalking", false);
    }

    private void MakeAttack() {
        if(_blackboard.PlayerInRange || _blackboard.TreeInRange) {
            Invoke(nameof(ApplyDamage), 0.3f);
            _animator.SetBool("isAttacking", true);
        } else {
            _behaviorTree.InterruptTree();
        }
    }

    private void ApplyDamage() {
        if(_target == _player) {
            _target.GetComponent<Unit>().HandleDamage(new DamageInstance(_dmg, DamageType.normal));
        } else {
            ProgressionManager.Instance.RemoveProgression(_dmg);
        }
        Invoke(nameof(OnFinish), 1);
        Invoke(nameof(ResetAttackAnimation), 0f);
    }
    private void ResetAttackAnimation() {
        _animator.SetBool("isAttacking", false);
    }

    private void InterruptAttack() {
        CancelInvoke(nameof(MakeAttack));
        CancelInvoke(nameof(ApplyDamage));
        CancelInvoke(nameof(OnFinish));
    }
}

