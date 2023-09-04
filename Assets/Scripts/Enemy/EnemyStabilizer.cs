using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStabilizer : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidBody;

    private void Update() {
        if (transform.position.y < -6f) {
            GameObject.Destroy(gameObject);
        }
    }

}
