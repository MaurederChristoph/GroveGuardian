using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PersistentVariables : Singleton<PersistentVariables> {
    public List<Mood> UnlockedMoods = new();
    public float Volume { get; private set; } = 1.0f;
    public void UnlockMode() {
        if(!(Enum.GetValues(typeof(Mood)).Length == 4)) {
            UnlockedMoods.Add((Mood)UnlockedMoods.Count);
        }
    }
}