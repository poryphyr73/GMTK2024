using UnityEngine.Audio;
using System;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public SFX[] sfx;

    private void Awake()
    {
        foreach(SFX s in sfx)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        SFX s = Array.Find(sfx, sound => sound.name == name);

        if (s == null) return;
        s.source.Play();
    }
}
