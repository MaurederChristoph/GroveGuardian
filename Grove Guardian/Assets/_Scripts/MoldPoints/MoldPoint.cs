using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemySpawnSets;

public class MoldPoint : Unit {
    [SerializeField] private Point _patrol;

    private int _removeAmount;
    private int _progressionBonus;
    private float _radius;
    private float _amountOfEnemies;
    private readonly List<GameObject> _units = new();
    private bool _canBeEnabled = false;
    
    private void Update() {
        if(_units.Count != 0) {
            _canBeEnabled = true;
        }
        if (_units.Count == 0 && _canBeEnabled) {
            _healthBar.transform.parent.gameObject.SetActive(true);
        }
    }

    public void Activate(int removeAmount, int progressionBonus, float radius, int amountOfEnemies) {
        _removeAmount = removeAmount;
        _progressionBonus = progressionBonus;
        _radius = radius;
        _amountOfEnemies = amountOfEnemies;
        ProgressionManager.Instance.RemoveConstantProgression(removeAmount);
        SpawnEnemies();
    }

    private void SpawnEnemies() {
        for(var i = 0;i < _amountOfEnemies;i++) {
            Vector2 randomPoint = Random.insideUnitCircle * _radius;
            Vector3 finalPosition = transform.position + new Vector3(randomPoint.x, 4, randomPoint.y);
            if(!NavMesh.SamplePosition(finalPosition, out NavMeshHit hit, 10, NavMesh.AllAreas)) {
                i--;
                continue;
            }
            var unit = SpawnManager.Instance.SpawnUnit(EnemyType.Protection, hit.position);
            _units.Add(unit);
            var enemy = unit.GetComponent<EnemyUnit>();
            enemy.SetNewPoint(_patrol);
            enemy.AddDeathListener((unit) => _units.Remove(unit));
            _patrol.gameObject.transform.localScale = Utils.MultiplyVectorWithPoint(Vector3.one, _radius);
            enemy.GetComponent<Blackboard>().ProtectPoint = true;
        }
    }

    public override void HandleDamage(DamageInstance damageInstance) {
        if(_units.Count > 0) {
            return;
        }
        base.HandleDamage(damageInstance);
    }

    private void OnDestroy() {
        OnDeath.Invoke(gameObject);
        var prog = ProgressionManager.Instance;
        if(prog != null) {
            prog.AddConstantProgression(_removeAmount);
            prog.AddProgression(_progressionBonus);
        }
    }

    protected override void HitEffect() {

    }
}
