using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemySpawnSet", fileName = "EnemySpawnSet")]
public class EnemySpawnSets : ScriptableObject {
    public List<EnemySet> Enemies;


    [Serializable]
    public class EnemySet {
        public int Standard;
        public int BigChongus;
        public int BabyZombie;

        public int Count() {
            return Standard + BabyZombie + BigChongus;
        }
        public List<int> GetList() {
            return new List<int> {
                Standard,
                BigChongus,
                BabyZombie
            };
        }
    }

    public enum EnemyType {
        Standard,
        BigChongus,
        BabyZombie,
        Protection,
        Mold,
    }
}
