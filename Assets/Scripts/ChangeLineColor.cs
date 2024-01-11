using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeLineColor : MonoBehaviour
{

    public List<GameObject> lines = new List<GameObject>();

    private void Update() {
        foreach(GameObject line in lines) {
            if(EventSystem.current.currentSelectedGameObject == gameObject) {
                line.GetComponent<UIPaletteChange>().PaletteColor = 2;
                EventBroker.CallPaletteChange();
            } else {
                line.GetComponent<UIPaletteChange>().PaletteColor = 1;
                EventBroker.CallPaletteChange();
            }
        }
    }
}
