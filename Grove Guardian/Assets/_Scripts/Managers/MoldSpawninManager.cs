using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MoldSpawningManager : MonoBehaviour {
    [Header("Balance")]
    [SerializeField] private int _progressionDecrease;
    [SerializeField] private int _progressionBonus;
    [SerializeField] private float _radius;
    [SerializeField] private int _amountOfEnemies;
    [SerializeField] private float _firstSpawnAfter;
    [SerializeField] private float _spawningIntervals;


    [Header("Setup")]
    [SerializeField] private GameObject _mold;
    [SerializeField] private Transform _moldParent;
    [SerializeField] private Transform[] _spawningPositions;



    private readonly List<Transform> _currentlyActive = new();
    private float _height;
    private GameObject _currentSpawn;
    private bool _isSpawning;
    private float _currentDistanceTraveled;


    private void Start() {
        if(_spawningPositions.Length != 0) {
            InvokeRepeating(nameof(SpawnMold), _firstSpawnAfter, _spawningIntervals);
        }
    }
    private void Update() {
        if(_isSpawning && _currentSpawn != null) {
            _currentSpawn.transform.position = new Vector3(_currentSpawn.transform.position.x, _currentSpawn.transform.position.y + 0.01f, _currentSpawn.transform.position.z);
            _currentDistanceTraveled += 0.01f;
            if(_currentDistanceTraveled > _height * 1.5) {
                _currentDistanceTraveled = 0;
                _currentSpawn.GetComponent<MoldPoint>().Activate(_progressionDecrease, _progressionBonus, _radius, _amountOfEnemies);
                _currentSpawn.GetComponent<Collider>().enabled = true;
                _isSpawning = false;
            }
        }
    }

    public void SpawnMold() {
        if(_isSpawning || (GameManager.Instance.State != GameManager.GameState.SpawnEnemies && GameManager.Instance.State != GameManager.GameState.Combat)) {
            return;
        }
        var rnd = new System.Random();
        var index = -1;
        if(_currentlyActive.Count() < _spawningPositions.Count()) {
            do {
                index = rnd.Next(_spawningPositions.Length);
            } while(_currentlyActive.Contains(_spawningPositions[index]));
        } else {
            Debug.LogWarning("Max Amount of Spawns reached");
            return;
        }
        var _originalPos = _spawningPositions[index];
        _currentlyActive.Add(_originalPos);

        _height = _mold.GetComponentInChildren<MeshRenderer>().bounds.size.y;
        Vector3 spawnPos = _originalPos.position;
        spawnPos.y -= _height;
        _currentSpawn = Instantiate(_mold, spawnPos, Quaternion.identity, _moldParent);
        _currentSpawn.GetComponent<MoldPoint>().AddDeathListener((GameObject go) => {
            _currentlyActive.Remove(_originalPos);
        });
        _isSpawning = true;
    }
}
