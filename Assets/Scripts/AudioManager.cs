using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    private void Start()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            foreach (Sound s in musicTracks)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.audioFile;
                s.source.volume = musicVolume;
                s.source.pitch = 1;
                s.source.playOnAwake = false;
            }
            foreach (Sound s in soundEffects)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.audioFile;
                s.source.volume = effectVolume;
                s.source.pitch = 1;
                s.source.playOnAwake = false;
            }
        }
        else
        {
            Destroy(this) ;
        }

        AudioManager.PlayMusic("hubSong");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayEffect(tryout);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayMusic(tryout);
        }
    }

    public string tryout;

    public float musicVolume = .5f; //should be serialized
    public static void SetMusicVolume(float _vol)
    {
        if (_instance == null)
            return;
        _instance.musicVolume = _vol;
        if (_instance.musicVolume < 0)
            _instance.musicVolume = 0;
        else if (_instance.musicVolume > 1)
            _instance.musicVolume = 1;
        if (_instance.musicTracks == null)
            return;
        foreach (var item in _instance.musicTracks)
        {
            item.source.volume = _instance.musicVolume;
        }
    }
    public Sound[] musicTracks;
    public float effectVolume = .5f;
    public static void SetEffectVolume(float _vol)
    {
        if (_instance == null)
            return;
        _instance.effectVolume = _vol;
        if (_instance.effectVolume < 0)
            _instance.effectVolume = 0;
        else if (_instance.effectVolume > 1)
            _instance.effectVolume = 1;
    }
    public Sound[] soundEffects;

    public static Sound PlayEffect(string _name)
    {
        if(_instance == null)
        {
            Debug.LogWarning("Sound manager not instantiated!");
            return null;
        }
        Sound s = Array.Find(_instance.soundEffects, sound => sound.soundName == _name);
        if(s == null || s.source == null)
        {
            Debug.LogWarning("Sound name not found or sound has no source!");
            return null;
        }
        s.source.loop = false;
        s.source.volume = _instance.effectVolume;
        s.source.Play();
        return s;
    }

    public static Sound PLayEffectLoop(string _name)
    {
        Sound s = PlayEffect(_name);
        if(s != null)
        {
            s.source.Stop();
            s.source.loop = true;
            s.source.Play();
        }
        return s;
    }

    public static void StopEffectLoop(string _name)
    {
        if (_instance == null)
        {
            Debug.LogWarning("Sound manager not instantiated!");
            return;
        }
        Sound s = Array.Find(_instance.musicTracks, sound => sound.soundName == _name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound name not found or sound has no source!");
            return;
        }
        s.source.Stop();
    }

    public static Sound PlayMusic(string _name)
    {
        if (_instance == null)
        {
            Debug.LogWarning("Sound manager not instantiated!");
            return null;
        }
        Sound s = Array.Find(_instance.musicTracks, sound => sound.soundName == _name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound name not found or sound has no source!");
            return null;
        }
        foreach (var item in _instance.musicTracks)
        {
            if(item.source.isPlaying)
                item.source.Stop();
        }
        s.source.loop = true;
        s.source.volume = _instance.musicVolume;
        s.source.Play();
        return s;
    }

    public static void StopMusic(string _name)
    {
        if(_instance != null && _instance.musicTracks != null)
        {
            foreach (var item in _instance.musicTracks)
            {
                item.source.Stop();
            }
        }
    }
}

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip audioFile;
    [HideInInspector]
    //[Range(0.0f, 1.0f)]
    public float volume = 0.7f;
    [HideInInspector]
    //[Range(1f, 3f)]
    public float pitch = 1f;

    [HideInInspector]
    public AudioSource source;
}
