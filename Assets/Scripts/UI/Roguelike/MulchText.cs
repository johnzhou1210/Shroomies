using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MulchText : MonoBehaviour
{
    TextMeshProUGUI _textComponent;
    [SerializeField] Animator _animator;

    private void Start() {
        _textComponent = GetComponent<TextMeshProUGUI>();
    }

    public void OnIncreaseMulch(int newVal) {
        _animator.StopPlayback();
        _animator.Play("MulchGain");
        _textComponent.text = newVal.ToString();
    }
}
