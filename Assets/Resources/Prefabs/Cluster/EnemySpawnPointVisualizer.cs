using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySpawnPoint))]
[ExecuteInEditMode]
public class EnemySpawnPointVisualizer : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;

    private void Start() {
        if (Application.isPlaying) {
            GameObject.Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying) {
            GameObject enemyPrefab = GetComponent<EnemySpawnPoint>().EnemyPrefab;
            if (enemyPrefab != null && _spriteRenderer != null) {
                _spriteRenderer.sprite = enemyPrefab.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite;
            }

        }
    }
}
