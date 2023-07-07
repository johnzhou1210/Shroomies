using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ClusterEditor : MonoBehaviour
{
    public Vector3 SpawnPosition;
    [SerializeField] Cluster _cluster;
    public float MovementSpeed;

    private void OnEnable() {
        SpawnPosition = transform.position;
    }

    void Update() {
        if (!Application.isPlaying) {
            SpawnPosition = transform.position;
        }
    }

    
}
