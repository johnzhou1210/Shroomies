using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class TapToStart : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _textMesh;

    public float blinkSpeed = 1.0f;
    public float opacityModifier = .1f;

    // Update is called once per frame
    void Update() {
        _textMesh.color = new Color(1,1,1, Mathf.Cos(Time.fixedTime * blinkSpeed) + .8f + opacityModifier);
    }
}
