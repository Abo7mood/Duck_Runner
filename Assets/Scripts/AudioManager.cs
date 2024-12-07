using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance;
    public float soundSpeed;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.Volume;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.AwakePlay ;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0) 
        {
            if (!sounds[5].source.isPlaying) sounds[5].source.Play();
            else StartCoroutine(VolumeDown(sounds[5].source));
        }
        if(scene.buildIndex == 1)
        {
            StartCoroutine(VolumeUp(sounds[5].source));
        }
        
    }
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        s.source.Play();
    }
    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        s.source.Stop();
    }
    IEnumerator VolumeUp(AudioSource sound)
    {
        while (sound.volume < 0.9)
        {
            sound.volume = Mathf.Lerp(sound.volume, 1f, soundSpeed);
            yield return null;
        }
        sound.volume = 1f;
        yield return null;
    }
    IEnumerator VolumeDown(AudioSource sound)
    {
        while (sound.volume > 0.4f)
        {
            sound.volume = Mathf.Lerp(sound.volume, 0.3f, soundSpeed);
            yield return null;
        }
        sound.volume = 0.3f;
        yield return null;
    }
}
