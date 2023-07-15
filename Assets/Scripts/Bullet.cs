using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Bullet : MonoBehaviour {
    Vector2 _moveDir;
    public float Velocity = 10f;
    public bool reflect = true;
    public BulletType Type;
    public BulletOwnershipType Ownership;
    public HashSet<IDamageable> HitTargets;
    public GameObject ExplosionPrefab;
    public int Damage = 1;
    readonly float _removeTime = 9f;

    [SerializeField] Rigidbody2D _rigidBody;

    Vector2 _lastVelocity = Vector2.zero;

    bool _debounce = false;

    private void OnEnable() {
        Invoke("Destroy", _removeTime);
        HitTargets = new HashSet<IDamageable>();
    }

    private void FixedUpdate() {
        //transform.Translate(Velocity * Time.deltaTime * _moveDir);
        //_lastFramePos = transform.position;
        _lastVelocity = _rigidBody.velocity;
        if (transform.position.y < -6 || transform.position.y > 5.8f) {
            Destroy();
        }
    }

    public void SetVelocity(Vector2 newVelocity) {
        _rigidBody.velocity = newVelocity;
    }

    public void Destroy() {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        _rigidBody.velocity = Vector2.zero;
        HitTargets.Clear();
        CancelInvoke();
    }


    void OnCollisionEnter2D(Collision2D collision) {
        GameObject hitTarget = collision.gameObject;

        if (hitTarget.CompareTag("Enemy") && (Ownership == BulletOwnershipType.ENEMY || Ownership == BulletOwnershipType.ALLY)
            ||
            hitTarget.CompareTag("Player") && (Ownership == BulletOwnershipType.PLAYER || Ownership == BulletOwnershipType.ALLY)) {
            // do nothing
        } else {
            if (_debounce == false) {
                // play hit sound
                _debounce = true;
            }

            if (hitTarget.CompareTag("Player")) {
                bool success = HitTargets.Add(hitTarget.GetComponent<PlayerOnHit>());
                if (success) {
                    hitTarget.GetComponent<PlayerOnHit>().takeDamage(Damage);
                    Debug.Log(hitTarget.name + " took " + Damage + " damage!");
                    Destroy();
                }
            } else if (hitTarget.CompareTag("Enemy")) {
                Debug.Log(hitTarget.GetComponent<EnemyOnHit>());
                bool success = HitTargets.Add(hitTarget.GetComponent<EnemyOnHit>());
                if (success) {
                    hitTarget.GetComponent<EnemyOnHit>().takeDamage(Damage);
                    Debug.Log(hitTarget.name + " took " + Damage + " damage!");
                    Destroy();
                }
            }

            if (!reflect) {
                Destroy();
            } else {

                // get the point of contact
                ContactPoint2D contact = collision.contacts[0];

                // reflect our old velocity off the contact point's normal vector
                Vector2 reflectedVelocity = Vector3.Reflect(_lastVelocity, contact.normal);

                // assign the reflected velocity back to the rigidbody
                _rigidBody.velocity = reflectedVelocity;

                // rotate the object by the same ammount we changed its velocity
                Quaternion rotation = Quaternion.FromToRotation(_lastVelocity, reflectedVelocity);
                transform.rotation = rotation * transform.rotation;




            }
        }




    }




}
