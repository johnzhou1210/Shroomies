using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {
    float _strength;
    float _duration;

    private Vector3 _initialCameraPosition;
    private float _remainingShakeTime;

    public void Shake(float howStrong, float howLong) {
        _duration = howLong;
        _strength = howStrong;
        _remainingShakeTime = _duration;
    }

    private void Awake() {
        _initialCameraPosition = transform.localPosition;
    }

    private void Update() {
        if (_remainingShakeTime <= 0) {
            transform.localPosition = _initialCameraPosition;
        } else {
            transform.Translate(Random.insideUnitCircle * _strength);

            _remainingShakeTime -= Time.deltaTime;
        }


    }
}