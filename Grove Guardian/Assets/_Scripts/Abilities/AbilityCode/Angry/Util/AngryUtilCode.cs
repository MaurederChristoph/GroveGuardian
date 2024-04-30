using UnityEngine;

public class AngryUtilCode : AbilityCodeBase {
    private GameObject _fire;
    private Transform _caster;
    private int _spawned = 0;
    private float _trailAmount;
    private float _range;
    private float _speed;
    private float _spawnTime;
    private float _duration;


    public void SetupAbility(GameObject fire, Transform caster, int damage, DamageType damageType, float trailAmount, float range, float speed, float duration) {
        CreateDamageInstance(damage, damageType);
        _caster = caster;
        _fire = fire;
        _trailAmount = trailAmount;
        _range = range;
        _speed = speed;
        _spawnTime = _range / _speed / _trailAmount;
        _duration = duration;
        SpawnTrail();
    }

    private void SpawnTrail() {
        if(_spawned >= _trailAmount) {
            return;
        }
        var fire = Instantiate(_fire, _caster.transform.position, Quaternion.identity, transform);
        Destroy(fire, _duration);
        _spawned++;
        Invoke(nameof(SpawnTrail), _spawnTime);
    }

    private void OnTriggerEnter(Collider other) {
        StartPersistenceHealthEffect(other, standardEnemies.ToArray());
    }

    private void OnTriggerExit(Collider other) {
        EndPersistenceHealthEffect(other, standardEnemies.ToArray());
    }

    private void OnDestroy() {
        DestroyPersistenceDamageEffect(standardEnemies.ToArray());
    }
}
