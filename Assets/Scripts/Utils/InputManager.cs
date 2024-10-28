using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;
    public static PlayerInputActions InputActions;
    public static event Action<InputActionMap> ChangeActionMap;

    private void Awake() {
        InputActions = new PlayerInputActions();
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "TitleScene")
            ToggleActionMap(InputActions.UI);
        else if (SceneManager.GetActiveScene().name == "GameScene")
            ToggleActionMap(InputActions.Player);
    }

    public static void ToggleActionMap(InputActionMap actionMap) {
        if (actionMap.enabled) {
            return;
        }

        InputActions.Disable();
        ChangeActionMap?.Invoke(actionMap);
        actionMap.Enable();
    }

    public static void ChangeControls(InputActionMap actionmap) {
        if (actionmap.enabled) {
            return;
        }

        if (actionmap.name == "Player") {
            InputActions.Player.Disable();
            InputActions.UI.Enable();

        } else if (actionmap.name == "UI") {
            InputActions.Player.Enable();
            InputActions.UI.Disable();
        }
    }
}