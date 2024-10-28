using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu, _pauseButton, _optionsMenu, CreditsMenu;
    [SerializeField] Sprite _buttonPressed, _buttonNormal;
    [SerializeField] GameObject _dragArea;

    private PlayerInputActions playerInputActions;
    public Button primaryButton, PauseMenuButton, OptionsMenuButton, CreditsMenuButton;
    public bool randomPalette = true;

    private void Start() {
        playerInputActions = InputManager.InputActions;
    }

    public void OnClick() {
        AudioManager.Instance.PlaySFX("UI Select Sound");
    }

    public void TogglePause() {
        Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
        _pauseMenu.SetActive(Time.timeScale == 0f ? true : false);
        _pauseButton.SetActive(Time.timeScale == 0f ? false : true);
        if (Time.timeScale == 1f ) {
            _dragArea.SetActive(true);
            InputManager.ToggleActionMap(InputManager.InputActions.Player);
        } else {
            _dragArea.SetActive(false);
            InputManager.ToggleActionMap(InputManager.InputActions.UI);
            primaryButton.Select();
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

    public void MenuToOptions() {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
        OptionsMenuButton.Select();
    }

    public void OptionsToMenu() {
        _optionsMenu.SetActive(false);
        _pauseMenu.SetActive(true);
        PauseMenuButton.Select();
    }

    public void MenuToCredits() {
        _pauseMenu.SetActive(false);
        CreditsMenu.SetActive(true);
        CreditsMenuButton.Select();
    }

    public void CreditsToMenu() {
        CreditsMenu.SetActive(false);
        _pauseMenu.SetActive(true);
        PauseMenuButton.Select();
    }

    public void ToggleRandomPalette() {
        if(randomPalette)
            randomPalette = false;
        else if(!randomPalette)
            randomPalette = true;
    }

    private void Update() {
        if (_pauseButton.activeInHierarchy || (!_pauseButton.activeInHierarchy && Time.timeScale == 0f)) {
            if (playerInputActions.Player.Pause.WasReleasedThisFrame() || playerInputActions.UI.Unpause.WasReleasedThisFrame()) {
                OnClick();
                _pauseButton.transform.Find("PauseImage").GetComponent<Image>().sprite = _buttonNormal;
                TogglePause();
            } else if (playerInputActions.Player.Pause.WasPressedThisFrame() || playerInputActions.UI.Unpause.WasPressedThisFrame()) {
                _pauseButton.transform.Find("PauseImage").GetComponent<Image>().sprite = _buttonPressed;
            }
        }
        
    }



    

}
