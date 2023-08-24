using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStabilizer : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidBody;

    private void Update() {
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.totalForce = Vector2.zero;
        _rigidBody.totalTorque = 0f;
        _rigidBody.angularVelocity = 0f;
    }

}
