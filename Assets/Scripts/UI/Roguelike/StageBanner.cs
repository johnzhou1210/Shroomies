using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageBanner : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] TextMeshProUGUI _textComponent;

    private void OnEnable()
    {
        StageLogic.OnCueStageBanner += OnBannerCue;
    }

    private void OnDisable()
    {
        StageLogic.OnCueStageBanner -= OnBannerCue;
    }

    public void OnBannerCue(string newText) {
        _textComponent.text = newText;
        _animator.Play("StageBannerFade");
    }


}
