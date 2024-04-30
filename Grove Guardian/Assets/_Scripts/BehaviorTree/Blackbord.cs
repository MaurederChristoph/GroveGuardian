using System;
using System.Reflection;
using UnityEngine;

public class Blackboard : MonoBehaviour {
    public bool CanSeePlayer = false;
    public bool PlayerInRange = false;
    public bool TreeInRange = false;
    public bool ProtectPoint = false;
    public bool IsOnPoint = false;
    public bool JustSawPlayer = false;
    public bool JustWasDamaged = false;

    internal bool Check(string check) {
        Type type = GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
        foreach (FieldInfo field in fields) {
            if (field.FieldType == typeof(bool) && field.Name.ToLower() == check.ToLower()) {
                return (bool)field.GetValue(this);
            }
        }
        return false;
    }
}
