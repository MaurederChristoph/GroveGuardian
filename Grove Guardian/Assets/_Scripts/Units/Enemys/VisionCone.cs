using UnityEngine;

public class VisionCone : PlayerDetection {
    [SerializeField] private GameObject _visionPoint;
    [SerializeField] private EnemyBehaviorTree _behaviorTree;
    [SerializeField] private float justSawTime = 2;


    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            CancelInvoke(nameof(ChangeJustSawPlayer));
            _blackboard.CanSeePlayer = true;
            _behaviorTree.InterruptTree();
            //if (CheckCone(other.gameObject, _visionPoint.transform)) {
            //}
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            _blackboard.CanSeePlayer = false;
            _blackboard.JustSawPlayer = true;
            _behaviorTree.InterruptTree();
            Invoke(nameof(ChangeJustSawPlayer), justSawTime);
        }
    }

    private void ChangeJustSawPlayer() {
        _blackboard.JustSawPlayer = false;
    }
}
