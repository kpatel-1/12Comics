using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioMixerGroup mixerGroup;

    public Sound[] sounds;

    private Sound sound;



    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    void Start()
    {

    }

    public void Play(string name)
    {
        StopAllCoroutines();
        if (sound != null)
        {
            StartCoroutine(EndSound());
        }
            

        sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Music " + name + " not found.");
            return;
        }
        StartCoroutine(StartSound());
    }

    public void Play2(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    private IEnumerator EndSound()
    {
        AudioSource oldSound = sound.source;
        while (oldSound.volume > 0)
        {
            oldSound.volume -= 0.02f;
            //yield return null;
            yield return new WaitForSeconds(0.2f);
        }
        oldSound.Stop();

    }

    private IEnumerator StartSound()
    {
        sound.source.Play();
        float volume = 0f;
        do
        {
            sound.source.volume = volume;
            volume += 0.02f;
            //yield return null;
            yield return new WaitForSeconds(0.2f);
        } while (sound.source.volume <= sound.volume);
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Stop();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found!");
            return;
        }
        s.source.Pause();
    }

    public void UnPause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found!");
            return;
        }
        s.source.UnPause();
    }



}