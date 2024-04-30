using UnityEngine;

public class ThirdPersonCam : Singleton<ThirdPersonCam> {
    [Header("References")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _playerObj;
    [SerializeField] private Rigidbody _rb;

    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _combatLookAt;


    [Header("CameraSettings")]
    [SerializeField] private GameObject _combatSettings;
    //[SerializeField] private GameObject _explorationSettings;

    public CameraStyle CurrentCameraStyle { get; private set; }

    public enum CameraStyle {
        Combat,
        Exploration
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //    SwitchCameraStyle(CameraStyle.Combat);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2)) {
        //    SwitchCameraStyle(CameraStyle.Exploration);
        //}

        Vector3 viewDir = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);
        _orientation.forward = viewDir.normalized;

        if(CurrentCameraStyle == CameraStyle.Exploration) {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDirection = (_orientation.forward * verticalInput) + (_orientation.right * horizontalInput);

            if(inputDirection != Vector3.zero) {
                _playerObj.forward = Vector3.Slerp(_playerObj.forward, inputDirection.normalized, Time.deltaTime * _rotationSpeed);
            }
        }

        if(CurrentCameraStyle == CameraStyle.Combat) {
            Vector3 dirToCombatLookAt = _combatLookAt.position - new Vector3(transform.position.x, _combatLookAt.position.y, transform.position.z);
            _orientation.forward = dirToCombatLookAt.normalized;

            _playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    //private void SwitchCameraStyle(CameraStyle newStyle) {
    //    _combatSettings.SetActive(false);
    //    _explorationSettings.SetActive(false);
    //    switch (newStyle) {
    //        case CameraStyle.Combat:
    //        _combatSettings.SetActive(true);
    //        CurrentCameraStyle = CameraStyle.Combat;
    //        break;
    //        case CameraStyle.Exploration:
    //        _explorationSettings.SetActive(true);
    //        CurrentCameraStyle = CameraStyle.Exploration;
    //        break;
    //    }
    //    //CameraChange.Invoke();
    //}
}
