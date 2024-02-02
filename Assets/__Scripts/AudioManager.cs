using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

[Icon("Assets/AudioFileIcon.png")]
public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
            
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = s.sourceObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.spatialBlend;
            s.source.time = s.RandomizeTime ? UnityEngine.Random.Range(0f, s.clip.length) : 0f;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name, float Volume = 0f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (Volume > 0) 
            s.source.volume = Volume;
        else if (s.volume == 0) 
            s.source.volume = 1f;
        else 
            s.source.volume = s.volume;

        s.source.time = s.RandomizeTime ? UnityEngine.Random.Range(0f, s.clip.length) : 0f;
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;

        s.source.Stop();
    }
}
