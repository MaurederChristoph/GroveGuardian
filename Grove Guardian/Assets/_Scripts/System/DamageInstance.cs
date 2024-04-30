public struct DamageInstance {
    public int Damage { get; private set; }
    public DamageType DamageType { get; private set; }

    public DamageInstance(int damage, DamageType damageType) {
        Damage = damage;
        DamageType = damageType;
    }
}

public enum DamageType {
    normal,
    heal,
}
