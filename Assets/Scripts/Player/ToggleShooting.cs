using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleShooting : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
    [SerializeField] PlayerShooting _playerShooting;
    ShroomiesUpgradeController _shroomieShooting;

    private void Start() {
        _shroomieShooting = GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomiesUpgradeController>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("pointer down");
        _playerShooting.Toggle = true;
        _shroomieShooting.Toggle = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        Debug.Log("pointer up");
        _playerShooting.Toggle = false;
        _shroomieShooting.Toggle = false;
    }
}

