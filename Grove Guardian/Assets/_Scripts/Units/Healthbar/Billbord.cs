using UnityEngine;

public class Billboard : MonoBehaviour {
    private Transform _cam;

    private void Start() {
        _cam = GameObject.Find("Main Camera").transform;
    }
    private void LateUpdate() {
        transform.LookAt(transform.position + (_cam.forward * -1));
    }
}
