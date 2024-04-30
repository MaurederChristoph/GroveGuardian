using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Happy/HappyMelee", fileName = "HappyMelee")]
public class HappyMeleeScriptable : MoodAbility {
    [SerializeField] private float _radius;
    [SerializeField] private float _lifeTime;
    [SerializeField] private int _damage;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private GameObject _aura;

    private Transform _caster;

    public override void InitializeAbility(GameObject Caster) {
        _caster = Caster.transform;
    }

    public override void Activate() {
        var aura = Instantiate(_aura);
        aura.GetComponent<HappyMeleeCode>().SetupAbility(_caster, _lifeTime, OnAbilityStop, _damage, _damageType, SoundName);
        Vector3 newScale = new(_radius, aura.transform.localScale.y, _radius);
        aura.transform.localScale = newScale;
    }
}
