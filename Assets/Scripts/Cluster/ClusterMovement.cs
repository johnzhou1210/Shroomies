using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClusterEditor))]
public class ClusterMovement : MonoBehaviour {
    ClusterEditor _settings;

    private void Start() {
        _settings = GetComponent<ClusterEditor>();
    }

    private void FixedUpdate() {
        if (_settings != null) {
            transform.Translate(_settings.MovementSpeed * Time.fixedDeltaTime * Vector3.down);
        }
    }

}
