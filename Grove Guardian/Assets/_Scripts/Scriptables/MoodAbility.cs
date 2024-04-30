using UnityEngine;

[CreateAssetMenu]
public abstract class MoodAbility : AbilityBase {
    public Mood Mood;
}

public enum Mood {
    Happiness,
    Sadness,
    Anger,
    Fear,
    Surprise,
    Disgust,
}
