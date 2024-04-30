using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Angry/BasicAngry", fileName = "BasicAngry")]
public class AngryBasicScriptable : MoodAbility {
    [Header("SetUp")]
    [SerializeField] private GameObject _hitbox;
    [SerializeField] private int _damage;
    [SerializeField] private float _range;
    [SerializeField] private float _lingerDuration;
    [SerializeField] private int _healOnHit;
    [SerializeField] private DamageType _damageType;

    private readonly Transform _castingPoint;
    private readonly PlayerController _controller;

    public override void Activate() {
        var hit = Instantiate(_hitbox, _castingPoint.transform.position, _controller.gameObject.transform.GetChild(0).rotation, _controller.gameObject.transform.GetChild(0));
        hit.transform.localPosition = new Vector3(hit.transform.localPosition.x, hit.transform.localPosition.y, hit.transform.localPosition.z + _range);
        var hitController = hit.GetComponent<AngryBasicCode>();
        hitController.SetupAbility(_controller, OnAbilityStop, _damage, _damageType, _lingerDuration, _healOnHit);
        OnAbilityStop?.Invoke();
    }
}
