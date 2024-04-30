using UnityEngine;

public class AttackCone : PlayerDetection {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            _blackboard.PlayerInRange = true;
        }
        if(other.CompareTag("Tree")) {
            _blackboard.TreeInRange = true;
            _blackboard.IsOnPoint = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            _blackboard.PlayerInRange = false;
        }
        if(other.CompareTag("Tree")) {
            _blackboard.TreeInRange = false;
            _blackboard.IsOnPoint = false;
        }
    }
}
