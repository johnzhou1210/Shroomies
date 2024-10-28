using UnityEngine;

[RequireComponent (typeof(EnemySpawnPointVisualizer))]
public class EnemySpawnPoint : MonoBehaviour {
    [Header("CHOOSE ENEMY TO SPAWN HERE")] public GameObject EnemyPrefab;

    private void Awake() {
        Instantiate(EnemyPrefab, transform.position, Quaternion.identity, transform.parent);

    }

}