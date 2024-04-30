using System;
using System.Collections.Generic;
using UnityEngine;


public class HappyRangedCode : AbilityCodeBase {
    private Transform _castingPoint;
    private float _range;
    private readonly List<Collider> _currentlyAffectedTargets = new();
    private Transform _cam;
    private void Update() {
        RotateRay(_cam);
    }

    private void RotateRay(Transform cam) {
        var hasHit = Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, _range);
        Vector3 target;
        if(hasHit) {
            if(Vector3.Distance(cam.position, hit.point) < 10) {
                target = cam.position + (cam.forward * 10);
            } else {
                target = hit.point;
            }
        } else {
            target = cam.position + (cam.forward * _range);
        }

        var direction = target - transform.position;
        var rotation = Quaternion.LookRotation(direction);
        var offsetRotation = Quaternion.Euler(new Vector3(0, -90, 0));
        transform.rotation = rotation * offsetRotation;
    }

    public void SetupAbility(Transform caster, float lifeTime, Action onAbilityStop, int damage, DamageType damageType, float range, SoundClips soundName) {
        CreateDamageInstance(damage, damageType);
        OnAbilityStop = onAbilityStop;
        _castingPoint = caster;
        _range = range;
        _cam = ThirdPersonCam.Instance.gameObject.transform;
        RotateRay(_cam);
        Invoke(nameof(AbilityEnd), lifeTime);
        SoundManager.Instance.PlayFollowingSound(GetOutermostParent(GetOutermostParent(_castingPoint.gameObject)), soundName, lifeTime);
        caster.GetComponent<PlayerController>().ScreenShake(lifeTime - 0.7f,10f);
        caster.GetComponent<PlayerController>().LookMovement(lifeTime - 0.7f);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")) {
            StartPersistenceHealthEffect(other, standardEnemies.ToArray());
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Enemy")) {
            EndPersistenceHealthEffect(other, standardEnemies.ToArray());
        }
    }

    private void OnDestroy() {
        DestroyPersistenceDamageEffect(standardEnemies.ToArray());
    }
}

