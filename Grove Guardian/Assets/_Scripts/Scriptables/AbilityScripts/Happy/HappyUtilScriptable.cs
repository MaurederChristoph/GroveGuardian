using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Happy/HappyUtil", fileName = "HappyUtil")]
public class HappyUtilScriptable : MoodAbility {
    [SerializeField] private float _radius;
    [SerializeField] private float _lifeTime;
    [SerializeField] private int _heal;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private GameObject _aura;

    private Transform _caster;

    public override void InitializeAbility(GameObject Caster) {
        _caster = Caster.transform;
    }

    public override void Activate() {
        var aura = Instantiate(_aura, _caster.position - new Vector3(0,1,0), Quaternion.identity);
        aura.GetComponent<HappyUtilCode>().SetupAbility(_caster, _lifeTime, OnAbilityStop, _heal, _damageType, SoundName);
        Vector3 newScale = new(_radius, aura.transform.localScale.y, _radius);
        aura.transform.localScale = newScale;
    }
}
