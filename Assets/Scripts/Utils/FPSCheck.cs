using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCheck : MonoBehaviour
{
    TextMeshProUGUI _text;

    void Start() {
        _text = GetComponent<TextMeshProUGUI>();
    }
    float _timer, _avgFramerate;
    [SerializeField] float _textUpdateFrequency;

    private void Update() {
        //Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
        float timelapse = Time.smoothDeltaTime;
        _timer = _timer <= 0 ? _textUpdateFrequency : _timer -= timelapse;
        if (_timer <= 0) {
            _avgFramerate = (int)(1f / timelapse);
        }
        _text.text = _avgFramerate + " FPS";
    }




}
