using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemySpawnSets;

public class EnemyUnit : Unit {
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private EnemyType _type;
    [SerializeField] private GameObject _mesh;
    [SerializeField] private Color _targetColor;
    [SerializeField] private float _transitionDuration;
    private readonly List<Material> _targetMaterials = new();
    private Color _originalColor;

    private Vector3 _currentDestination;
    private Point _point;
    private float _startSpeed = 0;
    private Vector3 _savedVelocity;

    private void Awake() {
        _point = GameManager.Instance.Point;
    }

    public override void Start() {
        base.Start();
        _rb = GetComponent<Rigidbody>();
        SetUpColor();
        _startSpeed = _agent.speed;
    }

    public void SetNewPoint(Point point) {
        _point = point;
        GetComponent<EnemyBrain>().SetPoint(point);
    }

    public void InterruptMovement() {
        if(_type == EnemyType.BigChongus) {
            return;
        }
        _agent.speed = 0;
        _agent.velocity /= 5;
        Invoke(nameof(ResetSpeed), 0.1f);
    }

    public void ResetSpeed() {
        _agent.speed = _startSpeed;
    }

    protected override void HitEffect() {
        StartCoroutine(ChangeColor());
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
        _originalColor = _targetMaterials[1].GetColor("_Color");
    }

    IEnumerator ChangeColor() {
        yield return LerpColor(_targetColor, _transitionDuration);
        yield return new WaitForSeconds(0.1f);
        yield return LerpColor(_originalColor, _transitionDuration * 2);
    }

    IEnumerator LerpColor(Color newColor, float duration) {
        float time = 0;
        while(time < duration) {
            time += Time.deltaTime;
            var interpolatedColor = Color.Lerp(_originalColor, newColor, time / duration);
            foreach(var material in _targetMaterials) {
                material.SetColor("_Color", interpolatedColor);
            }
            yield return null;
        }

        foreach(var material in _targetMaterials) {
            material.SetColor("_Color", newColor);
        }
    }
}
