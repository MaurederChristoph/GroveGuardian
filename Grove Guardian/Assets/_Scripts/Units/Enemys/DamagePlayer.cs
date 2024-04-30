using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {
    [SerializeField] private int _damage = 10;
    [SerializeField] private int _radius = 3;
    [SerializeField] private DamageType _damageType = DamageType.normal;
    private readonly List<GameObject> _currentlyAffectedTargets = new();
    public GameObject Player;

    private bool _playerInside = false;

    private void Start() {
        Player = GameObject.Find("Player");
    }

    private void Update() {
        if(Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position)) < _radius && !_playerInside) {
            PlayerTriggerEnter();
            _playerInside = true;
        }
        if(Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position)) > _radius && _playerInside) {
            PlayerTriggerExit();
            _playerInside = false;
        }
    }
    private void PlayerTriggerEnter() {
        Player.GetComponent<Unit>().StartContinuousDamage(new DamageInstance(_damage, _damageType));
        _currentlyAffectedTargets.Add(Player);
    }
    private void PlayerTriggerExit() {

        Player.GetComponent<Unit>().EndContinuousDamage(new DamageInstance(_damage, _damageType));
        _currentlyAffectedTargets.Remove(Player);

    }

    private void OnDestroy() {
        foreach(var target in new List<GameObject>(_currentlyAffectedTargets)) {
            if(target != null) {
                PlayerTriggerExit();
            }
        }
    }
}
