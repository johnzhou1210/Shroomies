using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour {
    [SerializeField] UnityEvent _onPlayerTap;

    public void startGame() {
        SceneManager.LoadScene(1);
    }

}
