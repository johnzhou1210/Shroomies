using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour, IPointerDownHandler {

    public bool tutorial = true;

    private PlayerInputActions playerInputActions;


    public void OnPointerDown(PointerEventData eventData) {
        GetComponent<AudioSource>().Play();
        if (tutorial) {
            tutorial = false;
            foreach(Transform child in gameObject.transform) {
                if (child != null && !child.GetComponent<TapToStart>()) {
                    child.gameObject.SetActive(false);
                }
            }
            gameObject.GetComponent<Image>().color = Color.white;
        } else if (!tutorial) {
            Application.targetFrameRate = 100;
            SceneManager.LoadScene(1);
        }
    }

    private void Update() {
        if (playerInputActions.UI.Enter.WasPressedThisFrame()) {
            GetComponent<AudioSource>().Play();
            if (tutorial) {
                tutorial = false;
                foreach (Transform child in gameObject.transform) {
                    if (child != null && !child.GetComponent<TapToStart>()) {
                        child.gameObject.SetActive(false);
                    }
                }
                gameObject.GetComponent<Image>().color = Color.white;
            } else if (!tutorial) {
                Application.targetFrameRate = 100;
                SceneManager.LoadScene(1);
                InputManager.ToggleActionMap(playerInputActions.Player);
            }
        }
    }

    private void Start() {
        Application.targetFrameRate = 30;
        playerInputActions = InputManager.inputActions;
    }

}
