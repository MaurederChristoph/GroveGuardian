using System.Collections;
using System.Linq;
using UnityEngine;
using static AudioClips;

public class SoundManager : Singleton<SoundManager> {
    [SerializeField] AudioClips _audioClip;

    public void PlaySoundAt(Vector3 position, SoundClips clip, float time = -1) {
        GameObject soundPlayer = new("One shot audio");
        soundPlayer.transform.position = position;
        PlaySound(soundPlayer, clip, PersistentVariables.Instance.Volume, time);
    }

    public void PlayFollowingSound(GameObject parent, SoundClips clip, float time = -1) {
        GameObject soundPlayer = new("One shot follow audio");
        soundPlayer.transform.parent = parent.transform;
        soundPlayer.transform.localPosition = Vector3.zero;
        PlaySound(soundPlayer, clip, PersistentVariables.Instance.Volume, time);
    }

    public void PlayRandomSoundLoop(GameObject soundPlayer, SoundClips clipType) {
        StartCoroutine(PlayRandomSoundLoopCoroutine(soundPlayer, clipType, PersistentVariables.Instance.Volume));
    }

    private IEnumerator PlayRandomSoundLoopCoroutine(GameObject parent, SoundClips clip, float volume) {
        while(true) {
            GameObject soundPlayer = new("One shot follow audio");
            soundPlayer.transform.parent = parent.transform;
            soundPlayer.transform.localPosition = Vector3.zero;
            var audioClip = PlaySound(soundPlayer, clip, volume, -1, true);
            yield return new WaitForSeconds(audioClip.Clip.length);
        }
    }

    private NamedClip PlaySound(GameObject soundPlayer, SoundClips clip, float volume, float time, bool loop = false) {
        var audioSource = (AudioSource)soundPlayer.AddComponent(typeof(AudioSource));
        var audioClip = GetClip(clip);
        audioSource.clip = audioClip.Clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume * audioClip.volume;
        audioSource.Play();
        var length = time;
        if(length == -1 && audioClip.Clip != null) {
            length = audioClip.Clip.length;
        }
        StartCoroutine(FadeOutAudio(audioSource, length, loop));
        try {
            Destroy(soundPlayer, audioClip.Clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
        } catch(System.Exception) { }
        return audioClip;
    }

    private IEnumerator FadeOutAudio(AudioSource audioSource, float clipLength, bool loop) {
        float fadeStartTime;
        float fadeOutDuration;
        if(loop) {
            fadeStartTime = clipLength * 0.1f;
            fadeOutDuration = clipLength * 0.9f;
        } else {
            fadeStartTime = clipLength * 0.3f;
            fadeOutDuration = clipLength * 0.7f;
        }

        yield return new WaitForSeconds(fadeStartTime);

        var fadeOutTimer = 0f;
        var originalVolume = audioSource ? audioSource.volume : 0f;

        while(audioSource != null && fadeOutTimer < fadeOutDuration) {
            fadeOutTimer += Time.deltaTime;
            var normalizedTime = fadeOutTimer / fadeOutDuration;

            if(audioSource != null) {
                audioSource.volume = originalVolume * (1 - Mathf.Pow(normalizedTime, 2));
            }
            yield return null;
        }

        if(audioSource != null) {
            audioSource.Stop();
        }
    }

    private NamedClip GetClip(SoundClips clip) {
        var clips = _audioClip.SavedAuidoClips.Where(c => c.Type == clip).ToList();
        var rnd = new System.Random();
        var i = rnd.Next(0, clips.Count());
        return clips[i];
    }
}