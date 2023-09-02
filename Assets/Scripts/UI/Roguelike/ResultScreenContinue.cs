using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResultScreenContinue : MonoBehaviour {
    public bool won = false;

    private void Update() {
        if (Input.GetMouseButtonUp(0) || Input.touchCount > 0) {
            if (won) {
                SceneManager.LoadScene(0);
            } else {
                SceneManager.LoadScene(1);
            }
        }
    }

}
