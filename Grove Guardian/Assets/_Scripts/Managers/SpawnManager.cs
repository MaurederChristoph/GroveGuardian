using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnemySpawnSets;

public class SpawnManager : Singleton<SpawnManager> {
    public List<EnemySet> enemies;

    public GameObject SpawnUnit(EnemyType toSpawn, Vector3 pos) {
        return SpawnUnit(toSpawn, pos, Quaternion.identity);
    }

    public GameObject SpawnUnit(EnemyType toSpawn, Vector3 pos, Quaternion rotation) {
        var unit = Instantiate(GetPrefab(toSpawn), pos, rotation);
        var enemyManager = EnemyManager.Instance;
        enemyManager.AddEnemyCount();
        unit.GetComponent<Unit>().AddDeathListener(enemyManager.RemoveEnemyCount);
        return unit;
    }

    private GameObject GetPrefab(EnemyType toSpawn) {
        return enemies.Where(e => e.EnemyType == toSpawn).FirstOrDefault().Enemy;
    }

    [Serializable]
    public class EnemySet {
        public EnemyType EnemyType;
        public GameObject Enemy;
    }
}
