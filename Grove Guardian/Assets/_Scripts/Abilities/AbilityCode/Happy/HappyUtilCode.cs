using System;
using UnityEngine;

public class HappyUtilCode : AbilityCodeBase {
    private Transform _player;

    public void SetupAbility(Transform player, float lifeTime, Action onAbilityStop, int damage, DamageType damageType, SoundClips soundName) {
        CreateDamageInstance(damage, damageType);
        OnAbilityStop = onAbilityStop;
        _player = player.transform;
        Invoke(nameof(AbilityEnd), lifeTime);
        SoundManager.Instance.PlayFollowingSound(gameObject, soundName, lifeTime);

    }

    private void OnTriggerEnter(Collider other) {
        StartPersistenceHealthEffect(other, "Player");
    }

    private void OnTriggerExit(Collider other) {
        EndPersistenceHealthEffect(other, "Player");
    }

    private void OnDestroy() {
        DestroyPersistenceDamageEffect("Player");
    }
}
