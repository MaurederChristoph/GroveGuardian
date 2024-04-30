using UnityEngine;

public class HappyBasicCode : AbilityCodeBase {
    private const float TIME_TO_DESTROY = 3f;
    private const float MAX_DISTANCE = 30f;

    private const float MAX_VALUE = 2f;
    private const float MIN_VALUE = 0.5f;

    private float _speed;

    private Vector3 _target;

    private Rigidbody _rb;
    private Transform _shootingPoint;
    private PlayerController _controller;


    private void OnEnable() {
        Destroy(gameObject, TIME_TO_DESTROY);
    }

    private void Start() {
        _rb = GetComponent<Rigidbody>();
        var direction = (_target - transform.position).normalized;
        _rb.velocity = direction * _speed;
    }

    public void SetupAbility(Vector3 target, int damage, DamageType damageType, float speed, Transform shootingPoint, SoundClips soundName, PlayerController controller) {
        CreateDamageInstance(damage, damageType);
        _target = target;
        _speed = speed;
        _shootingPoint = shootingPoint;
        _controller = controller;
        SoundManager.Instance.PlayFollowingSound(GetOutermostParent(_shootingPoint.gameObject), soundName);
    }

    private void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Player")) {
            HandleSingleDamage(other);
        }
    }

    private void OnDestroy() {
        var distance = Vector3.Distance(transform.position, _controller.transform.position);
        if(distance < MAX_DISTANCE) {
            var scaledValue = ScaleValue(distance, MAX_DISTANCE, MIN_VALUE, MAX_VALUE);
            _controller.ScreenShake(0.2f, scaledValue);
        }
    }

    float ScaleValue(float distance, float maxDistance, float minValue, float maxValue) {
        return Mathf.Lerp(maxValue, minValue, distance / maxDistance);
    }
}
