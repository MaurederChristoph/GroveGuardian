using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioClipNames))]
public class AudioWriter : Editor {
    AudioClipNames audioClip;
    readonly string filePath = "Assets/_Scripts/Audio/";
    readonly string fileName = "SoundClips";

    private void OnEnable() {
        audioClip = (AudioClipNames)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Save")) {
            EdiorMethods.WriteToEnum(filePath, fileName, audioClip.clips);
        }
    }
}
