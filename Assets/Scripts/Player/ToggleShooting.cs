using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleShooting : MonoBehaviour {
    [SerializeField] PlayerShooting _playerShooting;
    ShroomiesUpgradeController _shroomieShooting;

    private void Start() {
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
        if (Input.GetButtonDown("Fire")) {
            _playerShooting.Toggle = true;
            _shroomieShooting.Toggle = true;
        } else if (Input.GetButtonUp("Fire")) {
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

