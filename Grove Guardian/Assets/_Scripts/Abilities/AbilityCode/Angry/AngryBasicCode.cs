using System;
using UnityEngine;

public class AngryBasicCode : AbilityCodeBase {
    private Unit _unit;
    private float _lingerDuration;
    private int _heal;

    public void SetupAbility(Unit unit, Action onAbilityStop, int damage, DamageType damageType, float lingerDuration, int heal) {
        CreateDamageInstance(damage, damageType);
        OnAbilityStop = onAbilityStop;
        _unit = unit;
        _lingerDuration = lingerDuration;
        _heal = heal;
        Invoke(nameof(AbilityEnd), _lingerDuration);
    }

    private void OnTriggerEnter(Collider other) {
        HandleSingleDamage(other, false);
    }
}
