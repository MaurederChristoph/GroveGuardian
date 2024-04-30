using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVelocity : MonoBehaviour {
    void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
