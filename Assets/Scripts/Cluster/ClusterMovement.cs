using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClusterSettings))]
public class ClusterMovement : MonoBehaviour {
    ClusterSettings _settings;
    [SerializeField] bool _moveCluster = true;

    float _movementSpeed;

    private void Start() {
        _settings = GetComponent<ClusterSettings>();
        _movementSpeed = Random.Range(_settings.MinMovementSpeed, _settings.MaxMovementSpeed);
    }

    private void FixedUpdate() {
        if (_settings != null) {
            if (_moveCluster) {
                transform.Translate(_movementSpeed * Time.fixedDeltaTime * Vector3.down);
            }
            if (!hasAliveEnemies() || (onlyBrambleRemains() && transform.position.y < -6f)) {
                Destroy(gameObject);
            }
        }
    }

    bool hasAliveEnemies() {
        foreach (Transform child in transform) {
            if (child.gameObject.activeInHierarchy) {
                return true;
            }
        }
        return false;
    }

    bool onlyBrambleRemains() {
        foreach (Transform child in transform) {
            if (!child.CompareTag("Obstacle")) {
                return false;
            }
        }
        return true;
    }
    

}
