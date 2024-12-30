using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    public Sound[] MusicSounds, SFXSounds;
    public List<Sound> Playlist;
    public AudioSource MusicSource, ShootingSFXSource, OtherSFXSource;
    public GameSettings GameSettings;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    IEnumerator MusicTransition(Sound music) {
        if (MusicSource.isPlaying) {
            // fade out currently playing one to the new one
            for (; MusicSource.volume > 0;) {
                MusicSource.volume = MusicSource.volume - .05f;
                yield return new WaitForSeconds(.1f);
            }
        }
        if (music == null) {
            yield break;
        }
        MusicSource.clip = music.AudioClip;
        MusicSource.loop = music.Loop;
        MusicSource.volume = music.Volume;
        MusicSource.pitch = music.Pitch;
        MusicSource.Play();
        yield return null;
    }

    public void PlayMusic(string musicName) {
        Sound music = Array.Find(MusicSounds, s => s.Name == musicName);
        if (music == null) {
            Debug.Log("Music: " + musicName + " not found!");
            return;
        }
        // stop any music currently playing
        // give it a fade out effect
        StopAllCoroutines();
        StartCoroutine(MusicTransition(music));
    }

    public void PlayMistaDJ() {
        Sound music;
        if(GameSettings.currentTrack == Playlist.Count) {
            GameSettings.currentTrack = 0;
            music = Playlist[GameSettings.currentTrack];
            StopAllCoroutines();
            StartCoroutine(MusicTransition(music));
            GameSettings.currentTrack++;
        }
        else if(GameSettings.currentTrack < Playlist.Count) {
            music = Playlist[GameSettings.currentTrack];
            StopAllCoroutines();
            StartCoroutine(MusicTransition(music));
            GameSettings.currentTrack++;
        }
    }

    public void PlayShootingSFX(string SFXName) {
        Sound sfx = Array.Find(SFXSounds, s => s.Name == SFXName);
        if (sfx == null) {
            Debug.Log("SFX: " + SFXName + " not found!");
            return;
        }

        if (Time.time - sfx.LastTimePlayed >= sfx.Cooldown) { // to avoid sounds stacking volume
            ShootingSFXSource.clip = sfx.AudioClip;
            ShootingSFXSource.loop = sfx.Loop;
            ShootingSFXSource.volume = sfx.Volume;
            ShootingSFXSource.pitch = sfx.Pitch;
            ShootingSFXSource.PlayOneShot(ShootingSFXSource.clip, ShootingSFXSource.volume);
            sfx.LastTimePlayed = Time.time;
        }
    }

    public void PlaySFX(string SFXName, float customVolume = 0) {
        Sound sfx = Array.Find(SFXSounds, s => s.Name == SFXName);
        if (sfx == null) {
            Debug.Log("SFX: " + SFXName + " not found!");
            return;
        }

        if (Time.time - sfx.LastTimePlayed >= sfx.Cooldown) { // to avoid sounds stacking volume
            OtherSFXSource.clip = sfx.AudioClip;
            OtherSFXSource.loop = sfx.Loop;
            OtherSFXSource.volume = customVolume > 0f ? customVolume : sfx.Volume;
            OtherSFXSource.pitch = sfx.Pitch;
            OtherSFXSource.PlayOneShot(OtherSFXSource.clip, OtherSFXSource.volume);
            sfx.LastTimePlayed = Time.time;
        }


    }

    public void StopAllMusic(bool smooth) {
        if (!smooth) {
            MusicSource.Stop();
        } else {
            StartCoroutine(MusicTransition(null));
        }
        
    }

}
