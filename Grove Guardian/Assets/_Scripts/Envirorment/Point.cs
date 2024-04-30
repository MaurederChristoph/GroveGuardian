using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {

    [field: SerializeField] public List<Transform> PointBorder;

    void OnDrawGizmos() {
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        Gizmos.DrawSphere(transform.position, transform.localScale.x);
    }
}
