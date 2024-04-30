using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Angry/UtilAngry", fileName = "UtilAngry")]
public class AngryUtilScriptable : MoodAbility {
    [Header("SetUp")]
    [SerializeField] private GameObject _fireSpawnObject;
    [SerializeField] private GameObject _fire;
    [SerializeField] private int _damage;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private float _speed;
    [SerializeField] private float _range;
    [SerializeField] private float _trailAmount;
    [SerializeField] private float _duration;



    private Transform _spellParent;
    private PlayerController _controller;
    public override void InitializeAbility(GameObject Caster) {
        _controller = Caster.GetComponent<PlayerController>();
        _controller.GetCastingInfo(out _, out _spellParent);
    }


    public override void Activate() {
        _controller.Dashing(_speed, 0, _range);
        OnAbilityStop?.Invoke();
        var fire = Instantiate(_fireSpawnObject, _spellParent);
        var hitController = fire.GetComponent<AngryUtilCode>();
        hitController.SetupAbility(_fire, _controller.gameObject.transform, _damage, _damageType, _trailAmount, _range, _speed, _duration);
    }
}
