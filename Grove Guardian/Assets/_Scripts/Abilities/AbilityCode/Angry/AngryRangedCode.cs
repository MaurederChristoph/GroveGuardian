using System;
using UnityEngine;

public class AngryRangedCode : AbilityCodeBase {
    private Unit _unit;
    private Action _action;
    private float _lingerDuration;

    public void SetupAbility(Unit unit, Action onAbilityStop, int damage, DamageType damageType, float lingerDuration) {
        CreateDamageInstance(damage, damageType);
        _unit = unit;
        _action = onAbilityStop;
        _lingerDuration = lingerDuration;
        Invoke(nameof(AbilityEnd), _lingerDuration);
    }

    private void OnTriggerEnter(Collider other) {
        HandleSingleDamage(other, false);
    }
}
