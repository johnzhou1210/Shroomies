using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MulchText : MonoBehaviour
{
    TextMeshProUGUI _textComponent;
    [SerializeField] Animator _animator;

    private void Start() {
        _textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        StageLogic.OnUpdateMulch += UpdateMulchText;
    }

    private void OnDisable()
    {
        StageLogic.OnUpdateMulch -= UpdateMulchText;
    }

    public void UpdateMulchText(int newVal) {
        _animator.StopPlayback();
        _animator.Play("MulchGain");
        _textComponent.text = newVal.ToString();
    }
}
