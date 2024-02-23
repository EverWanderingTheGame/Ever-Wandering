using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DisallowMultipleComponent]
[ExecuteInEditMode]
[DefaultExecutionOrder(-19999)]
public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public Sound[] sounds;

    public static AudioManager instance;
    public static bool isMuted = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (Application.isPlaying)
        {
            DontDestroyOnLoad(gameObject);
        }

        deleteAudioSources();
        createAudioSources();
    }

    public void Play(string name, float Volume = 0f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (Volume > 0) s.source.volume = Volume;
        else if (s.volume == 0) s.source.volume = 1f;
        else s.source.volume = s.volume;

        if (isMuted) s.source.volume = 0f;

        s.source.time = s.RandomizeTime ? UnityEngine.Random.Range(0f, s.clip.length) : 0f;
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    public void deleteAudioSources()
    {
        foreach (Sound s in sounds)
        {
            if (s.source == null) return;

            GameObject source = s.sourceObject;

            DestroyImmediate(source.GetComponent<AudioSource>());
        }
    }

    public void createAudioSources()
    {
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

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;

        s.source.Stop();
    }

    public void stopAllSound()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public static void Mute()
    {
        isMuted = true;
    }

    public static void UnMute()
    {
        isMuted = false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    private Sound[] sounds;
    private string[] soundName;
    private int selectedSoundIndex = 0;
    private float volume = 1f;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AudioManager audioManager = (AudioManager)target;

        sounds = audioManager.sounds;
        soundName = new string[sounds.Length];
        for (int i = 0; i < sounds.Length; i++)
        {
            soundName[i] = sounds[i].name;
        }

        GUILayout.Label("In Editor Sound Tester", EditorStyles.boldLabel);

        volume = EditorGUILayout.Slider("", volume, 0f, 1f, GUILayout.Width(280));

        GUILayout.BeginHorizontal();
        selectedSoundIndex = EditorGUILayout.Popup("", selectedSoundIndex, soundName);
        if (GUILayout.Button("Play Sound"))
        {
            audioManager.stopAllSound();
            audioManager.Play(soundName[selectedSoundIndex], volume);
        }
        if (GUILayout.Button("Stop All Sounds"))
        {
            audioManager.stopAllSound();
        }
        GUILayout.EndHorizontal();
    }
}
#endif