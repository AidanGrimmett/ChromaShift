using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;
    public MultiSound[] multiSounds;
    private Dictionary<AudioSource, string> repeatedSounds;
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

        foreach (MultiSound ms in multiSounds)
        {
            ms.source = gameObject.AddComponent<AudioSource>();
            ms.source.volume = ms.volume;
            ms.source.loop = ms.loop;
        }
    }
    
    private void Start()
    {
        PlaySound("music");
    }

    //private void Update()
    //{
    //    if (repeatedSounds.Count > 0)
    //    {
    //        foreach (KeyValuePair<AudioSource, string> source in repeatedSounds)
    //        {
    //            if (!source.Key.isPlaying)
    //            {
    //                Debug.Log("playloop");
    //                Sound s = Array.Find(sounds, sound => sound.name == source.Value);
    //                source.Key.clip = s.clip;
    //                source.Key.loop = s.loop;
    //                source.Key.Play();
    //            }
    //        }
    //    }
    //}

    public void PlaySound(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Cannot find a sound called + " + soundName + "!");
            return;
        }

        ChangeVolume(s);

        s.source.Play();
    }

    public void PlayPositional(string soundName, Vector3 position)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Cannot find a sound called " + soundName + "!");
            return;
        }

        GameObject soundObject = new GameObject("AudioSource: " + soundName);
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = s.clip;
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = 0.1f;
        audioSource.maxDistance = 1f;

        ChangeVolume(s);

        audioSource.Play();

        // Automatically destroy the GameObject and the AudioSource component when the sound finishes playing
        Destroy(soundObject, audioSource.clip.length);
    }

    public void PlayPositionalRepeat(string soundName, Vector3 position, string repeatName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Cannot find a sound called " + soundName + "!");
            return;
        }

        GameObject soundObject = new GameObject("AudioSource: " + soundName);
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = s.clip;
        audioSource.spatialBlend = 1f; // 3D sound

        ChangeVolume(s);

        audioSource.Play();

        repeatedSounds.Add(audioSource, repeatName);
    }

    public void PlayRandom(string soundsName)
    {
        MultiSound s = Array.Find(multiSounds, multiSound => multiSound.name == soundsName);
        if (s == null)
        {
            Debug.LogWarning("Cannot find a multi sound called + " + soundsName + "!");
            return;
        }
        int clipToPlay = UnityEngine.Random.Range(0, s.clips.Length - 1);
        s.source.clip = s.clips[clipToPlay];
        s.source.pitch = 1 + UnityEngine.Random.Range(-s.pitchRange, s.pitchRange);

        ChangeVolume(s);

        s.source.Play();
    }

    public void RemoveRepeat(string name)
    {
        foreach (KeyValuePair<AudioSource, string> source in repeatedSounds)
        {
            if (source.Value == name)
            {
                repeatedSounds.Remove(source.Key);
            }
        }
        Debug.Log(repeatedSounds.Count);
    }

    public void ChangeVolume(Sound s)
    {
        if (s.name == "music")
        {
            s.source.volume = s.volume * PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            s.source.volume = s.volume * PlayerPrefs.GetFloat("SoundVolume");
        }
    }
    public void ChangeVolume(MultiSound s)
    {
        if (s.name == "music")
        {
            s.source.volume = s.volume * PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            s.source.volume = s.volume * PlayerPrefs.GetFloat("SoundVolume");
        }
    }
}
