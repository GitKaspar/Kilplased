using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioClipGroup")]

public class AudioClipGroup : ScriptableObject
{
    [Range(0, 1)]
    public float VolumeMin = 1;
    [Range(0, 1)]
    public float VolumeMax = 1;
    [Range(0, 2)]
    public float PitchMin = 1;
    [Range(0, 2)]
    public float PitchMax = 1;

    public float Cooldown = 0.1f;
    public bool Loop;

    public List<AudioClip> Clips;

    private float timestamp;

    private void OnEnable()
    {
        timestamp = 0;
    }
    public AudioSource Play()
    {
        if (AudioSourcePool.Instance == null) return null;
        return Play(AudioSourcePool.Instance.GetSource());
    }

    public AudioSource Play(AudioSource source)
    {
        if (timestamp > Time.time)
        {
            return null;
        }
        if (Clips.Count <= 0) return null;
        timestamp = Time.time + Cooldown;

        source.volume = Random.Range(VolumeMin, VolumeMax);
        source.pitch = Random.Range(PitchMin, PitchMax);
        source.clip = Clips[Random.Range(0, Clips.Count)];
        source.loop = Loop;
        source.Play();
        return source;
    }

    public void Stop(AudioSource source)
    {
        if (source.isPlaying)
        { source.Stop(); }

    }

    public void Stop()
    {
        if (AudioSourcePool.Instance == null) return;
        Stop(AudioSourcePool.Instance.GetSource());
    }

    public void SetVolume(float volume)
    {
        VolumeMin = volume;
        VolumeMax = volume;
    }
}