using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TapToStart : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _textMesh;

    public float blinkSpeed = 1.0f;
    public float opacityModifier = .1f;

    // Update is called once per frame
    void Update() {
        if (ChangePalette.Holder != null) {
            _textMesh.color = new Color(ChangePalette.Holder.color1.r, ChangePalette.Holder.color1.g, ChangePalette.Holder.color1.b, Mathf.Cos(Time.fixedTime * blinkSpeed) + .8f + opacityModifier);
        }
        
    }


    
}
