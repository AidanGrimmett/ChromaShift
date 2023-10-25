using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        PlaySound("music");
    }

    public void PlaySound(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Cannot find a sound called + " + soundName + "!");
            return;
        }
        s.source.Play();
    }

    public void PlayPositional(string soundName, Vector3 position)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Cannot find a sound called + " + soundName + "!");
            return;
        }
        s.source.transform.position = position;
        s.source.Play();
    }
    //public void Play3DSound(AudioClip clip, Vector3 position)
    //{
    //    audioSource.spatialBlend = 1;
    //    audioSource.transform.position = position;
    //    audioSource.clip = clip;
    //    audioSource.Play();
    //}
}
