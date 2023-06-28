using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCleanup : MonoBehaviour
{
    public float _cleanupTime = 1f;

    private void Start() {
        Invoke("clean", _cleanupTime);
    }

    void clean() {
        Destroy(gameObject);
    }

}
