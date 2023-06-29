using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    Vector2 _moveDir;
    public float Velocity = 10f;

    public BulletType Type;
    public BulletOwnershipType Ownership;

    public int Damage = 1;
    readonly float _removeTime = 9f;
    public bool Bounce = false;
    public bool Pierce = false;
    public HashSet<GameObject> HitTargets;
    public GameObject ExplosionPrefab;

    bool _debounce = false;

    private void OnEnable() {
        Invoke("Destroy", _removeTime);
        HitTargets = new HashSet<GameObject>();
    }

    private void FixedUpdate() {
        transform.Translate(Velocity * Time.deltaTime * _moveDir);
    }

    public void SetMoveDirection(Vector2 dir) {
        _moveDir = dir;
    }

    public void Destroy() {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        HitTargets = null;
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) { // player hit detection is done in player controller
        GameObject hitTarget = collision.gameObject;
        if (_debounce == false ) {
            // check tags
            if (CompareTag("Player")) {
                if (hitTarget.CompareTag("Enemy")) {
                    HitTargets.Add(hitTarget);
                }
            } else if (CompareTag("Enemy")) {
                if (hitTarget.CompareTag("Player")) {
                    HitTargets.Add(hitTarget);
                }
            }
            _debounce = true;
            Destroy();
        }
    }

}
