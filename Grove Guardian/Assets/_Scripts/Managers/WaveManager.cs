using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemySpawnSets;
using static GameManager;

public class WaveManager : Singleton<WaveManager> {
    [SerializeField] private float _startDelay;
    [SerializeField] private int _enemiesPerSpawn;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private float _delayAfterFinishSet;
    [SerializeField] private Transform[] _spawner;
    [SerializeField] private EnemySpawnSets _spawnSet;


    private List<int> _currentSpawnSet;
    private int CurrentWave => GameManager.Instance.CurrentWave -1;

    public void StartWaveSpawning() {
        WaveSetup();
        Invoke(nameof(SpawnWaveUnits), _startDelay);
    }

    private void WaveSetup() {
        var currentSetIndex = GetCurrentSet(CurrentWave);
        _currentSpawnSet = _spawnSet.Enemies[currentSetIndex].GetList();
        _currentSpawnSet = ChangeDifficulty(_currentSpawnSet);
    }

    private void SpawnWaveUnits() {
        for(var i = 0;i < _enemiesPerSpawn;i++) {
            Vector3 randomPoint = GetSpawnPoint();
            var spawn = GetRandomEnemy(ref _currentSpawnSet);
            if(spawn == null) {
                return;
            }
            SpawnManager.Instance.SpawnUnit((EnemyType)spawn, randomPoint);
        }
        Invoke(nameof(SpawnWaveUnits), _spawnInterval);
    }

    private Vector3 GetSpawnPoint() {
        var rnd = new System.Random();
        var currentSpawner = _spawner[rnd.Next(_spawner.Length)];
        var currentSpawnerBounds = currentSpawner.localScale / 2;
        var currentSpawnerUpperBounds = currentSpawner.position + currentSpawnerBounds;
        var currentSpawnerLowerBounds = currentSpawner.position - currentSpawnerBounds;
        var randomX = Random.Range(currentSpawnerLowerBounds.x, currentSpawnerUpperBounds.x);
        var randomZ = Random.Range(currentSpawnerLowerBounds.z, currentSpawnerUpperBounds.z);
        var randomPoint = new Vector3(randomX, currentSpawner.transform.position.y, randomZ);
        return randomPoint;
    }
    private int GetCurrentSet(int currentWave) {
        return currentWave < _spawnSet.Enemies.Count - 1 ? currentWave : 2;
    }

    private EnemyType? GetRandomEnemy(ref List<int> set) {
        if(ProgressionManager.Instance.IsFinished) {
            GameManager.Instance.ChangeState(GameState.Combat);
            return null;
        }
        if(set.TrueForAll(field => field == 0)) {
            if(!ProgressionManager.Instance.IsFinished) {
                WaveSetup();
                Invoke(nameof(SpawnWaveUnits), _delayAfterFinishSet);
            } else {
                GameManager.Instance.ChangeState(GameState.Combat);
            }
            return null;
        }
        var index = Random.Range(0, set.Count);
        while(set[index] == 0) {
            index = Random.Range(0, set.Count);
        }
        set[index]--;
        return (EnemyType)index;
    }

    private List<int> ChangeDifficulty(List<int> currentSpawnSet) {
        if(CurrentWave > 10) {
            currentSpawnSet[0] += CurrentWave;
            currentSpawnSet[1] += CurrentWave;
            currentSpawnSet[2] += CurrentWave;
        } else if(CurrentWave > 6) {
            currentSpawnSet[0] += CurrentWave - 4;
            currentSpawnSet[1] += CurrentWave - 2;
            currentSpawnSet[2] += CurrentWave - 2;
        } else if(CurrentWave > 4) {
            currentSpawnSet[0] += CurrentWave - 4;
            currentSpawnSet[1] += CurrentWave - 4;
            currentSpawnSet[2] += CurrentWave - 4;
        }
        return currentSpawnSet;
    }
}
