using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Happy/BasicHappy", fileName = "BasicHappy")]
public class HappyBasicScriptable : MoodAbility {
    [Header("Shooting")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletHitMissDistance = 100;
    [SerializeField] private int _damage;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private float _speed;
    [SerializeField] private PlayerController _controller;

    private Transform _castingPoint;
    private Transform _spellParent;

    public override void InitializeAbility(GameObject Caster) {
        PlayerController controller = Caster.GetComponent<PlayerController>();
        controller.GetCastingInfo(out _castingPoint, out _spellParent);
        _controller = controller;
    }

    public override void Activate() {
        var cam = ThirdPersonCam.Instance.gameObject.transform;
        var hasHit = Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, Mathf.Infinity);
        Vector3 target;
        if(hasHit) {
            if(Vector3.Distance(cam.position, hit.point) < 10) {
                target = cam.position + (cam.forward * 10);
            } else {
                target = hit.point;
            }
        } else {
            target = cam.position + (cam.forward * _bulletHitMissDistance);
        }
        var targetRotation = Quaternion.LookRotation(target - _castingPoint.position, Vector3.up);
        targetRotation *= Quaternion.Euler(0, -90, 0);
        GameObject bullet = Instantiate(_bulletPrefab, _castingPoint.transform.position, targetRotation, _spellParent);
        HappyBasicCode bulletController = bullet.GetComponent<HappyBasicCode>();
        bulletController.SetupAbility(target, _damage, _damageType, _speed, _castingPoint, SoundName, _controller);
        OnAbilityStop?.Invoke();
    }
}
