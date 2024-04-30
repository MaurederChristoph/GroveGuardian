using UnityEngine;

public class EnemyInterrupter : MonoBehaviour {
    [Header("Variable")]
    [SerializeField] private float _detectionRadius;

    [Header("SetUp")]
    [SerializeField] private EnemyBehaviorTree _tree;
    [SerializeField] private GameObject _player;


    private bool _playerInside;

    private void FixedUpdate() {
        CheckPlayerDistance();
    }

    private void CheckPlayerDistance() {
        if(Mathf.Abs(Vector3.Distance(transform.position, _player.transform.position)) < _detectionRadius && !_playerInside) {
            _playerInside = true;
            Interrupt();
        }
        if(Mathf.Abs(Vector3.Distance(transform.position, _player.transform.position)) > _detectionRadius && _playerInside) {
            _playerInside = false;
            Interrupt();
        }
    }


    private void Interrupt() {
        _tree.InterruptTree();
    }
}
