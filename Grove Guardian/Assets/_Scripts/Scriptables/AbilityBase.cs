using System;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject {
    [field: SerializeField] public string Name { get; protected set; }
    [field: SerializeField] public string Description { get; protected set; }
    [field: SerializeField] public Sprite Icon { get; protected set; }
    [field: SerializeField] public float Cooldown { get; protected set; }
    [field: SerializeField] public AbilitySlot Slot { get; protected set; }
    [field: SerializeField] public SoundClips SoundName { get; protected set; }
    [field: SerializeField] public string AnimationName { get; protected set; }

    public Action OnAbilityStop;
    public abstract void Activate();
    public virtual void InitializeAbility(GameObject Caster) { }
}

public enum AbilitySlot {
    Ranged,
    Melee,
    Utils,
    Basic,
    Special,
    Passive
}
