using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource musicPrefab;
    public AudioSource effectPrefab;

    [Space(10f)]

    public Sound[] music;
    public Sound[] effects;

    private Dictionary<Sound, AudioSource> audioSounds = new Dictionary<Sound, AudioSource>();

    //Singleton!
    public static AudioManager Singleton
    {
        get; private set;
    }

    private void Awake()
    {
        if (Singleton != null)
            DestroyImmediate(gameObject);
        else
            Singleton = this;

        SetupSounds();
    }

    private void Start()
    {
        PlayMusic("MainTheme");
    }

    public void PlayMusic(string name)
    {
        AudioSource source;

        for (int i = 0; i < music.Length; i++)
        {
            if (music[i].name == name)
            {
                source = audioSounds[music[i]];
                source.Play();
                return;
            }
        }
    }

    public void PlayEffect(string name)
    {
        AudioSource source;

        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == name)
            {
                source = audioSounds[effects[i]];
                source.Play();
                return;
            }
        }
    }

    public void StopMusic(string name)
    {
        AudioSource source;

        for (int i = 0; i < music.Length; i++)
        {
            if (music[i].name == name)
            {
                source = audioSounds[music[i]];
                source.Stop();
                return;
            }
        }
    }

    public void StopEffect(string name)
    {
        AudioSource source;

        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == name)
            {
                source = audioSounds[effects[i]];
                source.Stop();
                return;
            }
        }
    }

    public void SetMusicVolume (string name, float volume)
    {
        AudioSource source;

        for (int i = 0; i < music.Length; i++)
        {
            if (music[i].name == name)
            {
                source = audioSounds[music[i]];
                music[i].volume = volume;
                source.volume = volume;

                return;
            }
        }
    }

    public void SetEffectVolume (string name, float volume)
    {
        AudioSource source;

        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == name)
            {
                source = audioSounds[effects[i]];
                effects[i].volume = volume;
                source.volume = volume;

                return;
            }
        }
    }

    public void SetMusicPitch(string name, float pitch)
    {
        AudioSource source;

        for (int i = 0; i < music.Length; i++)
        {
            if (music[i].name == name)
            {
                source = audioSounds[music[i]];
                music[i].pitch = pitch;
                source.pitch = pitch;

                return;
            }
        }
    }

    public void SetEffectPitch(string name, float pitch)
    {
        AudioSource source;

        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == name)
            {
                source = audioSounds[effects[i]];
                effects[i].pitch = pitch;
                source.pitch = pitch;

                return;
            }
        }
    }

    private void SetupSounds()
    {
        foreach (Sound sound in music)
        {
            AudioSource source = Instantiate(musicPrefab,
                transform.position,
                transform.rotation,
                transform) as AudioSource;

            source.tag = "Music";

            source.clip = sound.clip;
            source.loop = sound.loop;
            source.volume = sound.volume;
            source.pitch = sound.pitch;

            audioSounds.Add(sound, source);
        }

        foreach (Sound sound in effects)
        {
            AudioSource source = Instantiate(effectPrefab,
                transform.position,
                transform.rotation,
                transform) as AudioSource;

            source.tag = "Effect";

            source.clip = sound.clip;
            source.loop = sound.loop;
            source.volume = sound.volume;
            source.pitch = sound.pitch;

            audioSounds.Add(sound, source);
        }
    }
}
