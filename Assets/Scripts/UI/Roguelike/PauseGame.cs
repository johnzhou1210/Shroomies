using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu, _pauseButton;
    [SerializeField] Sprite _buttonPressed, _buttonNormal;
    [SerializeField] GameObject _dragArea;
    
    public void OnClick() {
        AudioManager.Instance.PlaySFX("UI Select Sound");
    }

    public void TogglePause() {
        Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
        _pauseMenu.SetActive(Time.timeScale == 0f ? true : false);
        _pauseButton.SetActive(Time.timeScale == 0f ? false : true);
        if (Time.timeScale == 1f ) {
            _dragArea.SetActive(true);
        } else {
            _dragArea.SetActive(false);
        }
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

    private void Update() {
        if (_pauseButton.activeInHierarchy || (!_pauseButton.activeInHierarchy && Time.timeScale == 0f)) {
            if (Input.GetKeyUp(KeyCode.P)) {
                OnClick();
                _pauseButton.transform.Find("PauseImage").GetComponent<Image>().sprite = _buttonNormal;
                TogglePause();
            } else if (Input.GetKeyDown(KeyCode.P)) {
                _pauseButton.transform.Find("PauseImage").GetComponent<Image>().sprite = _buttonPressed;
            }
        }
        
    }

    

}
