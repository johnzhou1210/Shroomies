using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] 
    Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume")) {
            PlayerPrefs.SetFloat("musicVolume", 1f);
            Load();
        } else {
            Load();
        }
    }

    public void ChangeVolume() {
        AudioListener.volume = volumeSlider.value;

    }

    public void Load() {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    public void Save() {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    public void IncVol() {
        if (volumeSlider.value < 1) {
            volumeSlider.value += .1f;
            if (volumeSlider.value > 1) {
                volumeSlider.value = 1;
            }
        }
    }

    public void DecVol() {
        if (volumeSlider.value > 0) {
            volumeSlider.value -= .1f;
            if (volumeSlider.value < 0)
                volumeSlider.value = 0;
        }
    }
}
