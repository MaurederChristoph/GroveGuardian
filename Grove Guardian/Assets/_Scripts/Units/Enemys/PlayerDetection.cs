using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour {
    [SerializeField] protected Blackboard _blackboard;
    public bool CheckCone(GameObject player, Transform visionPoint) {
        Vector3 direction = player.transform.position - visionPoint.transform.position;
        if (Physics.Raycast(visionPoint.transform.position, direction, out RaycastHit hitInfo)) {
            if (hitInfo.collider.CompareTag("Player")) {
                return true;
            }
        }
        return false;
    }
}
