using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAmbianceSound : MonoBehaviour {
    [SerializeField] private List<SoundClips> _ambianceSounds;
    [SerializeField] private GameObject _player;

    void Start() {
        foreach (var sound in _ambianceSounds) {
            SoundManager.Instance.PlayRandomSoundLoop(_player, sound);
        }
    }
}
