using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] MusicSounds, SFXSounds;
    public AudioSource MusicSource, SFXSource;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        PlayMusic("Shroomies Next Spread");
    }

    public void PlayMusic(string musicName) {
        Sound music = Array.Find(MusicSounds, s => s.Name == musicName);
        if (music == null) {
            Debug.Log("Music: " + musicName + " not found!");
            return;
        }
        MusicSource.clip = music.AudioClip;
        MusicSource.loop = music.Loop;
        MusicSource.volume = music.Volume;
        MusicSource.pitch = music.Pitch;
        MusicSource.Play();
    }

    public void PlaySFX(string SFXName) {
        Sound sfx = Array.Find(SFXSounds, s => s.Name == SFXName);
        if (sfx == null) {
            Debug.Log("SFX: " + SFXName + " not found!");
            return;
        }


        SFXSource.clip = sfx.AudioClip;
        SFXSource.loop = sfx.Loop;
        SFXSource.volume = sfx.Volume;
        SFXSource.pitch = sfx.Pitch;

        if (Time.time - sfx.LastTimePlayed >= sfx.Cooldown) { // to avoid sounds stacking volume
            SFXSource.PlayOneShot(SFXSource.clip, SFXSource.volume);
            sfx.LastTimePlayed = Time.time;
        }

        
    }

}
