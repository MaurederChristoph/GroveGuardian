using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Happy/RangeHappy", fileName = "RangeHappy")]
public class HappyRangedScriptable : MoodAbility {
    [SerializeField] private float _length;
    [SerializeField] private float _duration;
    [SerializeField] private int _damage;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private GameObject _ray;

    private Transform _caster;
    private Transform _castingPoint;

    public override void InitializeAbility(GameObject Caster) {
        _caster = Caster.transform;
        PlayerController controller = Caster.GetComponent<PlayerController>();
        controller.GetCastingInfo(out _castingPoint,out _);
    }

    public override void Activate() {
        var ray = Instantiate(_ray, Vector3.zero, Quaternion.identity, _castingPoint);
        ray.GetComponent<HappyRangedCode>().SetupAbility(_caster, _duration, OnAbilityStop, _damage, _damageType, _length, SoundName);
        ray.transform.localPosition = Vector3.zero;
        Vector3 newScale = new(_length / 10, ray.transform.localScale.y, ray.transform.localScale.z);
        ray.transform.localScale = newScale;

    }
}
