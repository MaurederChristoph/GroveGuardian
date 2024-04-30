using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicHappy : MonoBehaviour {
    public float rotationSpeed;
    private void Update() {
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}
