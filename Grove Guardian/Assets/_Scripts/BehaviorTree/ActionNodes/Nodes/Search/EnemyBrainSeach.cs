using System;
using UnityEngine;

public partial class EnemyBrain {
    public void SearchForPlayer(Action<ReturnState> OnFinish, float searchTime) {
        _navMeshAgent.isStopped = false;
        _onFinish = OnFinish;
        _animator.SetBool("isWalking", true);
        InvokeRepeating(nameof(SetPlayerPos), 0, 0.3f);
        Invoke(nameof(InterruptTree), searchTime);
    }

    private void InterruptTree() {
        _behaviorTree.InterruptTree();
    }

    private void SetPlayerPos() {
        _navMeshAgent.SetDestination(_player.position);
    }

    private void InterruptSearch() {
        _animator.SetBool("isWalking", false);
        CancelInvoke(nameof(SetPlayerPos));
        CancelInvoke(nameof(_behaviorTree.InterruptTree));
    }
}
