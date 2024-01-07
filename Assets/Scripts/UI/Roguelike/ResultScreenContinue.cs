using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResultScreenContinue : MonoBehaviour {
    public bool Won = false;

    private void Update() {
        if (InputManager.inputActions.UI.Enter.WasPressedThisFrame() || Input.touchCount > 0) {
            if (Won) {
                AudioManager.Instance.StopAllMusic(false);
                SceneManager.LoadScene(0);
            } else {
                AudioManager.Instance.StopAllMusic(false);
                SceneManager.LoadScene(1);
            }
        }
    }

}
