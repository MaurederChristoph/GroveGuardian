using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [field: SerializeField] public Slider Slider { get; private set; }

    public void SetHealth(int health) {
        if(health < 0) {
            Slider.value = 0;
            return;
        }
        Slider.value = health;
    }

    public void SetMaxHealth(int health) {
        if(health < 1) {
            throw new InvalidOperationException("Health is smaller the 0");
        }
        Slider.maxValue = health;
        Slider.value = health;
    }
}
