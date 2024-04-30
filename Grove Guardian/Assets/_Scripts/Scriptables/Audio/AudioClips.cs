using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundObject", fileName = "New SoundObject")]
public class AudioClips : ScriptableObject {
    public List<NamedClip> SavedAuidoClips;
    //[ArrayElementTitle("Name")] 
    [Serializable]
    public struct NamedClip {
        public SoundClips Type;
        public AudioClip Clip;
        [Range(0f, 1f)]
        public float volume;
    }
}

public class ArrayElementTitleAttribute : PropertyAttribute {
    public string Varname;
    public ArrayElementTitleAttribute(string ElementTitleVar) {
        Varname = ElementTitleVar;
    }
}
