using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageBanner : MonoBehaviour
{
    Animator _animator;
    [SerializeField] TextMeshProUGUI _textComponent;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    public void OnBannerCue(string newText) {
        _textComponent.text = newText;
        _animator.Play("StageBannerFade");
    }


}
