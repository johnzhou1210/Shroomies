using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleShooting : MonoBehaviour {
    
    [SerializeField] private PlayerShooting playerShooting;
    private ShroomiesUpgradeController shroomieShooting;
    private PlayerInputActions playerInputActions;

    private void Start() {
        playerInputActions = InputManager.InputActions;
        shroomieShooting = GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomiesUpgradeController>();
    }


#if UNITY_EDITOR || UNITY_2022_1_OR_NEWER
    void Update() {
        if (playerInputActions.Player.Fire.WasPressedThisFrame()) {
            playerShooting.Toggle = true;
            shroomieShooting.Toggle = true;
        } else if (playerInputActions.Player.Fire.WasReleasedThisFrame()) {
            playerShooting.Toggle = false;
            shroomieShooting.Toggle = false;
        }
    }
#elif UNITY_ANDROID || UNITY_IOS
void Update() {
        if (Input.touchCount > 0) {
            playerShooting.Toggle = true;
            shroomieShooting.Toggle = true;
        } else {
            playerShooting.Toggle = false;
            shroomieShooting.Toggle = false;
        }
    }
#endif







}

