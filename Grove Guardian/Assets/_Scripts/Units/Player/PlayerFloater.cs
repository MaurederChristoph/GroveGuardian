using UnityEngine;

public class PlayerFloater : MonoBehaviour {
    public float floatingHeight = 1.0f;
    public float floatSpeed = 0.5f;
    public float floatRange = 0.1f;
    public PlayerController playerController;
    private bool _IsJumping;

    private void Start() {
        playerController.OnJump += SetJump;
    }
    private void SetJump(bool canJump) {
        Debug.Log(canJump);
        _IsJumping = canJump;
    }

    void Update() {
        if(!_IsJumping) {
            RaycastHit hit;
            if(Physics.Raycast(playerController.Origin.position, Vector3.down, out hit)) {
                float targetHeight = hit.point.y + floatingHeight;
                float newY = targetHeight + Mathf.Sin(Time.time * floatSpeed) * floatRange;

                Vector3 finalPosition = new Vector3(transform.position.x, newY, transform.position.z);

                transform.position = finalPosition;
            }
        }
    }
}
