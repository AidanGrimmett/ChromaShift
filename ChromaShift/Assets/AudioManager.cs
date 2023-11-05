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
        //PlaySound("music");
    }

    private void Update()
    {
        foreach (MultiSound ms in multiSounds)
        {
            if (ms.name == "music")
            {
                if (!ms.source.isPlaying && MenuManager.gameState == GameState.Play)
                {
                    ms.source.clip = Array.Find(multiSounds, multiSound => multiSound.name == ms.name + "B").clips[ms.clipIndex];
                    ms.source.Play();
                }
            }
        }
        foreach (Sound s in sounds)
        {
            //if (s.name == "laserbuzz")
            //{
            //    if (!s.source.isPlaying)
            //    {
            //        //s.source.clip = Array.Find(sounds, sound => sound.name == s.name + "B").clip;
            //        s.source.Play();
            //    }
            //}
        }
    }

    public void PlaySound(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Cannot find a sound called + " + soundName + "!");
            return;
        }

        if (soundName.Contains("music"))
        {
            if (s.source.isPlaying)
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

    public void PlayRandom(string soundsName)
    {
        MultiSound s = Array.Find(multiSounds, multiSound => multiSound.name == soundsName);
        if (s == null)
        {
            Debug.LogWarning("Cannot find a multi sound called + " + soundsName + "!");
            return;
        }
        s.clipIndex = UnityEngine.Random.Range(0, s.clips.Length - 1);
        s.source.clip = s.clips[s.clipIndex];
        s.source.pitch = 1 + UnityEngine.Random.Range(-s.pitchRange, s.pitchRange);

        ChangeVolume(s);

        s.source.Play();
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

    public void StopSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name.Contains(name))
            {
                sounds[i].source.Stop();
            }
        }

        for (int i = 0; i < multiSounds.Length; i++)
        {
            if (multiSounds[i].name.Contains(name))
            {
                multiSounds[i].source.Stop();
            }
        }
    }

    public void PlaySpecificMulti(string soundsName, int index)
    {
        MultiSound s = Array.Find(multiSounds, multiSound => multiSound.name == soundsName);
        if (s == null)
        {
            Debug.LogWarning("Cannot find a multi sound called + " + soundsName + "!");
            return;
        }
        s.clipIndex = index;
        s.source.clip = s.clips[s.clipIndex];
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
        if (s.name.Contains("music"))
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
        if (s.name.Contains("music"))
        {
            s.source.volume = s.volume * PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            s.source.volume = s.volume * PlayerPrefs.GetFloat("SoundVolume");
        }
    }

    public void SetMusicVol(float f)
    {
        PlayerPrefs.SetFloat("MusicVolume", f);
    }

    public void SetSoundVol(float f)
    {
        PlayerPrefs.SetFloat("SoundVolume", f);
    }
}
