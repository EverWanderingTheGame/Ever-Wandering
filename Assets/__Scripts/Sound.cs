using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public GameObject sourceObject;

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float volume;
    [Range(.1f, 3f)] public float pitch;
    [Range(0f, 1f)] public float spatialBlend;
    public bool RandomizeTime = false;
    public bool loop = false;

    [HideInInspector] public AudioSource source;
}
