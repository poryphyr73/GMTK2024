using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;

        Play(name, s.volume, s.pitch, s.loop);
    }

    public void Play(string name, float volume, float pitch, bool loop)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;

        source.clip = s.clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.Play();
    }
}
