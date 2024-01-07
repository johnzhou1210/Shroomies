using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;
    public static PlayerInputActions inputActions;
    public static event Action<InputActionMap> actionMapchange;

    private void Awake() {
        inputActions = new PlayerInputActions();
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "TitleScene")
            ToggleActionMap(inputActions.UI);
        else if (SceneManager.GetActiveScene().name == "GameScene")
            ToggleActionMap(inputActions.Player);
    }

    public static void ToggleActionMap(InputActionMap actionMap) {
        if (actionMap.enabled) {
            return;
        }

        inputActions.Disable();
        actionMapchange?.Invoke(actionMap);
        actionMap.Enable();
    }

    public static void ChangeControls(InputActionMap actionmap) {
        if (actionmap.enabled) {
            return;
        }

        if (actionmap.name == "Player") {
            inputActions.Player.Disable();
            inputActions.UI.Enable();

        } else if (actionmap.name == "UI") {
            inputActions.Player.Enable();
            inputActions.UI.Disable();
        }
    }
}