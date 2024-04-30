using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Angry/RangedAngry", fileName = "RangedAngry")]
public class AngryRangedScriptable : MoodAbility {
    [Header("SetUp")]
    [SerializeField] private GameObject _hitbox;
    [SerializeField] private int _damage;
    [SerializeField] private float _lingerDuration;
    [SerializeField] private DamageType _damageType;

    private Transform _castingPoint;
    private Transform _spellParent;
    private PlayerController _controller;
    public override void InitializeAbility(GameObject Caster) {
        _controller = Caster.GetComponent<PlayerController>();
        _controller.GetCastingInfo(out _castingPoint, out _spellParent);
    }

    public override void Activate() {
        var rot = _controller.gameObject.transform.GetChild(0).rotation * Quaternion.Euler(90, 0, 0);
        var hit = Instantiate(_hitbox, _castingPoint.transform.position, rot, _spellParent);
        var hitController = hit.GetComponent<AngryRangedCode>();
        hitController.SetupAbility(_controller, OnAbilityStop, _damage, _damageType, _lingerDuration);
        OnAbilityStop?.Invoke();
    }
}
