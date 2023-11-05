using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class MultiSound
{
    public string name;

    public AudioClip[] clips;

    public int clipIndex;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;
    [Range(0f, 0.15f)]
    public float pitchRange;

    [HideInInspector]
    public AudioSource source;
}
