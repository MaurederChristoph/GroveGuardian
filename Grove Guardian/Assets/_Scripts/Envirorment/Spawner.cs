using UnityEngine;

public class Spawner : MonoBehaviour {
    void OnDrawGizmos() {
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
