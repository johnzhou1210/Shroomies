using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu, _pauseButton;
    
    public void OnClick() {
        AudioManager.Instance.PlaySFX("UI Select Sound");
    }

    public void TogglePause() {
        Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
        _pauseMenu.SetActive(Time.timeScale == 0f ? true : false);
        _pauseButton.SetActive(Time.timeScale == 0f ? false : true);
    }

    public void ReturnToMenu() {
        Time.timeScale = 1f;
        AudioManager.Instance.StopAllMusic(false);
        SceneManager.LoadScene(0);
    }

    public void Restart() {
        Time.timeScale = 1f;
        AudioManager.Instance.StopAllMusic(false);
        SceneManager.LoadScene(1);
    }

}
