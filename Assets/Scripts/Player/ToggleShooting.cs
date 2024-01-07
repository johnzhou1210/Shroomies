using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleShooting : MonoBehaviour {
    private PlayerInputActions playerInputActions;

    [SerializeField] PlayerShooting _playerShooting;
    ShroomiesUpgradeController _shroomieShooting;

    private void Start() {
        playerInputActions = InputManager.inputActions;
        _shroomieShooting = GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomiesUpgradeController>();
    }

    //public void OnPointerDown(PointerEventData eventData) {
    //    Debug.Log("pointer down");
    //    _playerShooting.Toggle = true;
    //    _shroomieShooting.Toggle = true;
    //}

    //public void OnPointerUp(PointerEventData eventData) {
    //    Debug.Log("pointer up");
    //    _playerShooting.Toggle = false;
    //    _shroomieShooting.Toggle = false;
    //}


#if UNITY_EDITOR || UNITY_2022_1_OR_NEWER
    void Update() {
        if (playerInputActions.Player.Fire.WasPressedThisFrame()) {
            _playerShooting.Toggle = true;
            _shroomieShooting.Toggle = true;
        } else if (playerInputActions.Player.Fire.WasReleasedThisFrame()) {
            _playerShooting.Toggle = false;
            _shroomieShooting.Toggle = false;
        }
    }
#elif UNITY_ANDROID || UNITY_IOS
void Update() {
        if (Input.touchCount > 0) {
            _playerShooting.Toggle = true;
            _shroomieShooting.Toggle = true;
        } else {
            _playerShooting.Toggle = false;
            _shroomieShooting.Toggle = false;
        }
    }
#endif







}

