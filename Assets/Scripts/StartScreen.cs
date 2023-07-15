using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour, IPointerDownHandler {
    public void OnPointerDown(PointerEventData eventData) {
        Application.targetFrameRate = 100;
        SceneManager.LoadScene(1);
    }

    private void Start() {
        Application.targetFrameRate = 30;
    }

}
