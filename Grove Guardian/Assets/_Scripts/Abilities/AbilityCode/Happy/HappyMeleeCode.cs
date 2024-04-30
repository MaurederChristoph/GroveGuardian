using System;
using System.Collections.Generic;
using UnityEngine;

public class HappyMeleeCode : AbilityCodeBase {
    private Transform _player;
    private readonly List<Collider> _currentlyAffectedTargets = new();

    private void Update() {
        transform.position = _player.position;
    }

    public void SetupAbility(Transform player, float lifeTime, Action onAbilityStop, int damage, DamageType damageType, SoundClips soundName) {
        CreateDamageInstance(damage, damageType);
        OnAbilityStop = onAbilityStop;
        _player = player.transform;
        Invoke(nameof(AbilityEnd), lifeTime);
        SoundManager.Instance.PlayFollowingSound(GetOutermostParent(_player.gameObject), soundName, lifeTime);

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
