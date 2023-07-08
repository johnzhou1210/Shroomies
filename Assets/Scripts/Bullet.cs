using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    public HashSet<IDamageable> HitTargets;
    public GameObject ExplosionPrefab;

    bool _debounce = false;

    private void OnEnable() {
        Invoke("Destroy", _removeTime);
        HitTargets = new HashSet<IDamageable>();
    }

    private void FixedUpdate() {
        transform.Translate(Velocity * Time.deltaTime * _moveDir);
        if (transform.position.y < -6 || transform.position.y > 6) {
            Destroy();
        }
    }

    public void SetMoveDirection(Vector2 dir) {
        _moveDir = dir;
    }

    public void Destroy() {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        HitTargets.Clear();
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) { // player hit detection is done in player controller
        GameObject hitTarget = collision.gameObject;

        if (hitTarget.CompareTag("Enemy") && (Ownership == BulletOwnershipType.ENEMY || Ownership == BulletOwnershipType.ALLY)
            ||
            hitTarget.CompareTag("Player") && (Ownership == BulletOwnershipType.PLAYER || Ownership == BulletOwnershipType.ALLY)) {
            // do nothing
        } else {
            Debug.Log("bullet hit " + hitTarget.name);
            if (_debounce == false) {
                // play hit sound
                _debounce = true;
            }

            if (hitTarget.CompareTag("Player")) {
                bool success = HitTargets.Add(hitTarget.GetComponent<PlayerOnHit>());
                if (success) {
                    hitTarget.GetComponent<PlayerOnHit>().takeDamage(Damage);
                    Debug.Log(hitTarget.name + " took " + Damage + " damage!");
                }
            } else if (hitTarget.CompareTag("Enemy")) {
                Debug.Log(hitTarget.GetComponent<EnemyOnHit>());
                bool success = HitTargets.Add(hitTarget.GetComponent<EnemyOnHit>());
                if (success) {
                    hitTarget.GetComponent<EnemyOnHit>().takeDamage(Damage);
                    Debug.Log(hitTarget.name + " took " + Damage + " damage!");
                }
            }
            Destroy();
        }
    }

}
