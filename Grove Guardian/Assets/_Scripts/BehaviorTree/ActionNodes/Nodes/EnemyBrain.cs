using System;
using UnityEngine;

public partial class EnemyBrain : MonoBehaviour {
    [SerializeField] public EnemyBehaviorTree _behaviorTree;
    [SerializeField] private Animator _animator;
    private Transform _player;
    private Transform _tree;
    private bool _isInterrupted { get; set; } = false;
    private Action<ReturnState> _onFinish;
    private Action _onUpdate;
    public Action OnInterrupt { get; set; }

    private void Awake() {
        _player = GameManager.Instance.Player.transform;
        _tree = GameManager.Instance.Tree.transform;
        _point = GameManager.Instance.Point;
        AddInterrupts();
    }

    private void AddInterrupts() {
        OnInterrupt += InterruptWalk;
        OnInterrupt += InterruptSearch;
        OnInterrupt += InterruptAttack;
    }

    private void FixedUpdate() {
        _onUpdate?.Invoke();
    }

    private void OnFinish() {
        _onFinish.Invoke(ReturnState.success);
    }
}
