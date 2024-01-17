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
    public List<GameObject> tutorialPages = new List<GameObject>();

    private PlayerInputActions playerInputActions;
    [SerializeField] int page = 0;


    public void OnPointerDown(PointerEventData eventData) {
        /*GetComponent<AudioSource>().Play();
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
        }*/
    }

    private void Update() {
        if (playerInputActions.UI.Enter.WasPressedThisFrame()) {
            GetComponent<AudioSource>().Play();
            if (page != 3) {
                foreach (Transform child in gameObject.transform) {
                    if (child != null && !child.GetComponent<TapToStart>()) {
                        child.gameObject.SetActive(false);
                    }
                }
                tutorialPages[page].SetActive(true);
                page++;
            }   else if (page == 3) {
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
