using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MulchText : MonoBehaviour
{
    TextMeshProUGUI _textComponent;

    private void Start() {
        _textComponent = GetComponent<TextMeshProUGUI>();
    }

    public void OnIncreaseMulch(int newVal) {
        _textComponent.text = newVal.ToString();
    }
}
