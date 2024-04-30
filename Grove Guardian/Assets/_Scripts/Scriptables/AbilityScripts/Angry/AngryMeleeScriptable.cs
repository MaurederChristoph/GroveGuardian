using UnityEngine;

public class AngryMeleeScriptable : MoodAbility {
    [Header("SetUp")]
    [SerializeField] private GameObject _hitbox;
    [SerializeField] private int _damage;
    [SerializeField] private float _lingerDuration;
    [SerializeField] private DamageType _damageType;

    public override void Activate() {
        throw new System.NotImplementedException();
    }
}
