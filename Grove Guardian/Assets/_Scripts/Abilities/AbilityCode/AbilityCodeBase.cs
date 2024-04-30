using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCodeBase : MonoBehaviour {
    private readonly List<Collider> _currentlyAffectedTargets = new();
    protected DamageInstance DamageInstance { get; private set; }
    protected Action OnAbilityStop;
    protected List<string> standardEnemies = new() { "Enemy", "Mold" };
    private readonly GameManager _gameManager = GameManager.Instance;

    public void CreateDamageInstance(int damage, DamageType damageType) {
        _gameManager.OnNewWave += UpdateDamageInstance;
        DamageInstance = new DamageInstance(damage, damageType);
    }

    public void UpdateDamageInstance() {
        DamageInstance = new DamageInstance(DamageInstance.Damage + _gameManager.PlayerDamageBonus, DamageInstance.DamageType);
    }

    protected void HandleSingleDamage(Collider other, bool destroyOnEnvironment = true) {
        if(other != null) {
            if(other.CompareTag("Detection")) {
                return;
            }
            if(other.CompareTag("Enemy")) {
                other.gameObject.GetComponent<Unit>().HandleDamage(DamageInstance);
                Destroy(gameObject);
            }
            if(other.CompareTag("Mold")) {
                other.gameObject.GetComponent<MoldPoint>().HandleDamage(DamageInstance);
                Destroy(gameObject);
            }
            if(destroyOnEnvironment && other.CompareTag("Environment")) {
                Destroy(gameObject);
            }
        }
    }

    protected void StartPersistenceHealthEffect(Collider other, params string[] effectedTargets) {
        Unit otherUnit = other.GetComponent<Unit>();
        if(ShouldApplyEffect(otherUnit, other, effectedTargets)) {
            otherUnit.StartContinuousDamage(DamageInstance);
            _currentlyAffectedTargets.Add(other);
        }
    }

    protected void EndPersistenceHealthEffect(Collider other, params string[] effectedTargets) {
        Unit otherUnit = other.GetComponent<Unit>();
        if(ShouldApplyEffect(otherUnit, other, effectedTargets)) {
            otherUnit.EndContinuousDamage(DamageInstance);
            _currentlyAffectedTargets.Remove(other);
        }
    }

    protected void DestroyPersistenceDamageEffect(params string[] effectedTargets) {
        foreach(var target in new List<Collider>(_currentlyAffectedTargets)) {
            if(target != null) {
                Debug.Log("Removing EndPersistenceHealthEffect");
                EndPersistenceHealthEffect(target, effectedTargets);
            }
        }
    }

    private bool ShouldApplyEffect(Unit otherUnit, Collider other, params string[] tags) {
        return otherUnit != null && other.CompareTags(tags);
    }

    public virtual void AbilityEnd() {
        OnAbilityStop?.Invoke();
        Destroy(gameObject);
    }

    protected GameObject GetOutermostParent(GameObject childObject) {
        Transform currentTransform = childObject.transform;
        while(currentTransform.parent.name != "Units") {
            currentTransform = currentTransform.parent;
        }

        return currentTransform.gameObject;
    }
}
