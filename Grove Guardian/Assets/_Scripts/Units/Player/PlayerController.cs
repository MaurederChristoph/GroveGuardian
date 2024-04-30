using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Unit {

    [SerializeField] private Transform _orientation;
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    public bool _readyToJump = true;
    private bool _jumped = false;
    private float _horizontalInput;
    private float _verticalInput;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _groundRadius = 0.1f;
    [field: SerializeField] public Transform Origin { get; private set; }
    public bool _grounded;

    [Header("Casting")]
    [SerializeField] private Transform _castingStartPoint;
    [SerializeField] private Transform _parentOfSpell;
    [SerializeField] private CinemachineFreeLook _cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin[] _noiseSettings = new CinemachineBasicMultiChannelPerlin[3];

    [Header("Material")]
    [SerializeField] private GameObject _mesh;
    [SerializeField] private float _transitionDuration;
    [SerializeField] private Color _targetColor;
    private readonly List<Material> _targetMaterials = new();
    private Material _targetMaterial;
    private Color _originalColor;
    private bool _canMove = true;



    private Vector3 _moveDirection;
    private Rigidbody _rb;
    private bool _isDashing = false;

    public Action<bool> OnJump;

    override public void Start() {
        base.Start();
        SetUpColor();
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        for(var i = 0;i < 3;i++) {
            _noiseSettings[i] = _cinemachineVirtualCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    private void SetUpColor() {
        SkinnedMeshRenderer[] meshRenderers = _mesh.GetComponentsInChildren<SkinnedMeshRenderer>();
        if(meshRenderers.Length == 0) {
            return;
        }
        foreach(var meshRenderer in meshRenderers) {
            Material newMat = new Material(meshRenderer.material);
            meshRenderer.material = newMat;
            _targetMaterials.Add(newMat);
        }
        _originalColor = _targetMaterials[1].GetColor("_BaseColor");
    }

    void Update() {
        if(_canMove) {
            _grounded = Physics.OverlapSphere(Origin.position, _groundRadius, _whatIsGround).Count() != 0;
            if(!_isDashing) {
                MovementInput();
                SpeedControl();
            }
        } else {
            _rb.velocity = Vector3.zero;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    #region Movement
    private void MovementInput() {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(KeyCode.Space) && _readyToJump && _grounded) {
            _readyToJump = false;
            _jumped = true;
            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void MovePlayer() {
        _moveDirection = (_orientation.forward * _verticalInput) + (_orientation.right * _horizontalInput);
        if(_moveDirection != Vector3.zero) {
            if(_grounded) {
                _rb.AddForce(_moveSpeed * 15f * _moveDirection.normalized, ForceMode.Force);
            } else {
                _rb.AddForce(_moveSpeed * 10f * _moveDirection.normalized, ForceMode.Force);
            }
        } else if(_rb.velocity != Vector3.zero) {
            var newVelocity = Utils.MultiplyVectorWithPoint(_rb.velocity, 0.7d);
            newVelocity.y = _rb.velocity.y;
            _rb.velocity = newVelocity;
        }
    }

    private void SpeedControl() {
        Vector3 flatVel = new(_rb.velocity.x, 0f, _rb.velocity.z);

        if(flatVel.magnitude > _moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump() {
        OnJump?.Invoke(true);
        Invoke(nameof(EndJumpCheck), 0.5f);
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void EndJumpCheck() {
        if(_jumped && _grounded) {
            OnJump?.Invoke(false);
            _jumped = false;
            return;
        }
        Invoke(nameof(EndJumpCheck), 0.1f);
    }

    private void ResetJump() {
        _readyToJump = true;
    }
    #endregion

    #region Dashing
    public void Dashing(float dashForce, float dashUpwardForce, float dashDuration) {
        _isDashing = true;
        _horizontalInput = 1;
        gameObject.layer = LayerMask.NameToLayer("DashingPlayer");
        Vector3 forceToApply = (10 * dashForce * _orientation.forward) + (_orientation.up * dashUpwardForce);
        _rb.AddForce(forceToApply, ForceMode.Impulse);
        Invoke(nameof(ResetDash), dashDuration / 10);
    }

    private void ResetDash() {
        _isDashing = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    #endregion

    #region CastingInfo
    public void GetCastingInfo(out Transform casting) {
        casting = _castingStartPoint;
    }

    public void GetCastingInfo(out Transform casting, out Transform parent) {
        casting = _castingStartPoint;
        parent = _parentOfSpell;
    }
    #endregion

    private void OnDestroy() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
    }

    #region ColorChange
    protected override void HitEffect() {
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor() {
        yield return LerpColor(_targetColor, _transitionDuration);
        yield return new WaitForSeconds(0.1f);
        yield return LerpColor(_originalColor, 0.3f);
    }

    IEnumerator LerpColor(Color newColor, float duration) {
        float time = 0;
        while(time < duration) {
            time += Time.deltaTime;
            var interpolatedColor = Color.Lerp(_originalColor, newColor, time / duration);
            foreach(var material in _targetMaterials) {
                material.SetColor("_BaseColor", interpolatedColor);
            }
            yield return null;
        }

        foreach(var material in _targetMaterials) {
            material.SetColor("_BaseColor", newColor);
        }
    }
    #endregion

    #region screen shake

    public void ScreenShake(float duration, float magnitude) {
        StartCoroutine(Shake(duration, magnitude / 100));
    }
    public void ScreenShake(float duration, float magnitude, float amp) {
        StartCoroutine(Shake(duration, magnitude / 100, amp));
    }

    private IEnumerator Shake(float duration, float magnitude, float amp) {
        foreach(var noise in _noiseSettings) {
            noise.m_FrequencyGain = amp;
            noise.m_AmplitudeGain = magnitude;
        }

        yield return new WaitForSeconds(duration);

        foreach(var noise in _noiseSettings) {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 4f;
        }
    }

    private IEnumerator Shake(float duration, float magnitude) {
        foreach(var noise in _noiseSettings) {
            noise.m_AmplitudeGain = magnitude;
        }

        yield return new WaitForSeconds(duration);

        foreach(var noise in _noiseSettings) {
            noise.m_AmplitudeGain = 0f;
        }
    }
    #endregion region

    public void LookMovement(float duration) {
        Invoke(nameof(ResetMovement), duration);
        _canMove = false;
    }

    private void ResetMovement() {
        _canMove = true;
    }
}
