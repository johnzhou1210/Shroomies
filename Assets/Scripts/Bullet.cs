using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    Vector2 moveDir;
    public float velocity = 10f;

    public BulletType type;
    public BulletOwnershipType ownership;

    public int damage = 1;
    readonly float removeTime = 9f;
    public bool bounce = false;
    public bool pierce = false;
    public HashSet<GameObject> hitTargets;
    public GameObject explosionPrefab;

    bool _debounce = false;

    private void OnEnable() {
        Invoke("Destroy", removeTime);
        hitTargets = new HashSet<GameObject>();
    }

    private void FixedUpdate() {
        transform.Translate(velocity * Time.deltaTime * moveDir);
    }

    public void SetMoveDirection(Vector2 dir) {
        moveDir = dir;
    }

    public void Destroy() {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        hitTargets = null;
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) { // player hit detection is done in player controller
        GameObject hitTarget = collision.gameObject;
        if (_debounce == false ) {
            // check tags
            if (CompareTag("Player")) {
                if (hitTarget.CompareTag("Enemy")) {
                    hitTargets.Add(hitTarget);
                }
            } else if (CompareTag("Enemy")) {
                if (hitTarget.CompareTag("Player")) {
                    hitTargets.Add(hitTarget);
                }
            }
            _debounce = true;
            Destroy();
        }
    }

}
