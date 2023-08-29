using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ClusterSettings : MonoBehaviour
{
    public Vector3 SpawnPosition;
    public float MinMovementSpeed, MaxMovementSpeed;
    [HideInInspector] public float MovementSpeed;
    public float NextClusterMinDelay = 3f, NextClusterMaxDelay = 7f;

    private void OnEnable() {
        SpawnPosition = transform.position;
    }

    private void Start() {
        MovementSpeed = Random.Range(MinMovementSpeed, MaxMovementSpeed);
    }

    void Update() {
        if (!Application.isPlaying) {
            SpawnPosition = transform.position;
        }
    }

    
}
