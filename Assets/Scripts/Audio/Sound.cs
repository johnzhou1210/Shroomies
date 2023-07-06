using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class Sound : ScriptableObject {
    [Header("Basic Settings")]
    public string Name;
    public AudioClip AudioClip;
    public bool Loop;
    [Range(0,2f)] public float Volume = .5f;
    [Range(0,2f)] public float Pitch = 1f;

    [Header("Advanced Settings")]
    public float Cooldown = .01f;
    public float LastTimePlayed;

    private void OnEnable() {
        LastTimePlayed = Time.time;
    }

}
