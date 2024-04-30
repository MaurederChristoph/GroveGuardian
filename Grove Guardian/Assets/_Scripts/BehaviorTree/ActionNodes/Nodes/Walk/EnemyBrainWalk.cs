using System;
using UnityEngine.AI;
using UnityEngine;
using JetBrains.Annotations;

public partial class EnemyBrain {
    private bool _isMoving = false;
    private NavMeshAgent _navMeshAgent;
    private Point _point;

    public void StartWalkingTo(Action<ReturnState> OnFinish, Destination destination) {
        _animator.SetBool("isWalking", true);
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.isStopped = false;
        _onFinish = OnFinish;
        SetDestination(destination);
        _isMoving = true;
        if (destination == Destination.Player) {
            InvokeRepeating(nameof(UpdateWalkToPlayer), 0.05f, 0.01f);
        }
        _onUpdate += OnWalkUpdate;
    }

    private void SetDestination(Destination destination) {
        switch (destination) {
            case Destination.Player:
            _navMeshAgent.SetDestination(_player.position);
            break;
            case Destination.Tree:
            _navMeshAgent.SetDestination(_tree.position);
            break;
            case Destination.Mold:
            _navMeshAgent.SetDestination(GetDirection());
            break;
        }
    }

    private void UpdateWalkToPlayer() {
        SetDestination(Destination.Player);
    }

    void OnWalkUpdate() {
        if (_isMoving && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) {
            _isInterrupted = false;
            _isMoving = false;
            _navMeshAgent.isStopped = true;
            _navMeshAgent.velocity /= 5;
            _animator.SetBool("isWalking", false);
            CancelInvoke(nameof(UpdateWalkToPlayer));
            _onUpdate -= OnWalkUpdate;
            OnFinish();
        }
        if(_isInterrupted) {
            _isInterrupted = false;
            _isMoving = false;
            CancelInvoke(nameof(UpdateWalkToPlayer));
            _onUpdate -= OnWalkUpdate;
        }
    }

    private Vector3 GetDirection() {
        var randomPoint = EnemyManager.Instance.GetRandomPointInSphere(_point);
        if(NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 10, NavMesh.AllAreas)) {
            return hit.position;
        }
        Debug.Log("GetDirection failed");
        return transform.position;
    }

    public void InterruptWalk() {
        _isInterrupted = true;
        OnWalkUpdate();
    }

    public void SetPoint(Point point) {
        _point = point;
    }
}
