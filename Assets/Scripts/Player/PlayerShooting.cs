using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    bool _toggle = false;
    float _cooldown = 1f;
    float _fireRate = .25f;

    private void Start() {
        PlayerEventManager.Instance.OnSingleTap += onSingleTap;
        PlayerEventManager.Instance.OnDoubleTap += onDoubleTap;
    }

    void onSingleTap() {

    }

    void onDoubleTap() {
        Debug.Log("In here");
        _toggle = !_toggle;
    }

    private void Update() {
        _cooldown = Mathf.Clamp(_cooldown - Time.deltaTime, 0, _fireRate);
        if (_toggle && _cooldown <= 0f) {
            // shoot bullet
            StartCoroutine(fire());
            // reset cooldown
            _cooldown = _fireRate;
        }
    }

    IEnumerator fire() {
        Debug.Log("Shot bullet");
        GetComponent<AudioSource>().Play();
        yield return null;
    }

}
