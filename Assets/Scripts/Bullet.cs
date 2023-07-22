using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Bullet : MonoBehaviour {
    Vector2 _moveDir;
    float _velocity = 10f, _critRate = 0;
    bool _reflect = true;
    int _damage = 1, _pierceLimit = 32;
    public BulletType Type;
    public BulletOwnershipType Ownership;
    public HashSet<IDamageable> HitTargets;
    [SerializeField] GameObject _explosionPrefab, _critEffect;

    readonly float _removeTime = 9f;
    int _pierceCounter = 0;

    [SerializeField] Rigidbody2D _rigidBody;

    Vector2 _lastVelocity = Vector2.zero;

    bool _debounce = false;

    private void OnEnable() {
        Invoke("Destroy", _removeTime);
        HitTargets = new HashSet<IDamageable>();
        _pierceCounter = 0;
    }

    private void FixedUpdate() {
        //transform.Translate(Velocity * Time.deltaTime * _moveDir);
        //_lastFramePos = transform.position;
        _lastVelocity = _rigidBody.velocity;
        if (transform.position.y < -6 || transform.position.y > 5.5f) {
            Destroy();
        }
    }

    public void SetVelocity(Vector2 newVelocity) {
        _rigidBody.velocity = newVelocity;
    }

    public void Destroy() {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        _rigidBody.velocity = Vector2.zero;
        HitTargets.Clear();
        CancelInvoke();
    }


    void OnCollisionEnter2D(Collision2D collision) {
        GameObject hitTarget = collision.gameObject;
        if (hitTarget.gameObject.CompareTag("Enemy")) {
            hitTarget.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (hitTarget.CompareTag("Enemy") && (Ownership == BulletOwnershipType.ENEMY || Ownership == BulletOwnershipType.ALLY)
            ||
            hitTarget.CompareTag("Player") && (Ownership == BulletOwnershipType.PLAYER || Ownership == BulletOwnershipType.ALLY)) {
            // do nothing
        } else {
            if (_debounce == false) {
                // play hit sound
                _debounce = true;
            }

            if (hitTarget.CompareTag("Player") || hitTarget.CompareTag("Enemy")) {
                IDamageable onHit = hitTarget.CompareTag("Player") ? hitTarget.gameObject.GetComponent<PlayerOnHit>() : hitTarget.GetComponent<EnemyOnHit>();
                bool success = HitTargets.Add(onHit);
                if (success) {
                    // deal damage, account for crit rate.
                    if (Random.Range(0f, 1f) <= _critRate) {
                        _damage *= 2;
                        Instantiate(_critEffect, collision.transform.position, Quaternion.identity);
                        AudioManager.Instance.PlaySFX("Critical Hit Sound");
                        // do camera shake
                        Camera.main.GetComponent<CameraShaker>().Shake(.0225f, .1f);
                    }
                    onHit.takeDamage(_damage);
                    Debug.Log(hitTarget.name + " got hit");
                }
                Debug.Log(hitTarget.name + " took " + _damage + " damage!");

                

                if (_pierceLimit <= 0 || _pierceCounter >= _pierceLimit) {
                    Destroy();
                } else {
                    Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                    _pierceCounter++;
                    // ignore collisions with this target from now on
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);
                }
               
            } else if (hitTarget.CompareTag("Obstacle")) {
                // hit obstacle
            }


            if (_reflect && !hitTarget.CompareTag("Player") && !hitTarget.CompareTag("Enemy")) {
                // bounce off contact point
                ContactPoint2D contact = collision.contacts[0];
                // reflect our old velocity off the contact point's normal vector
                Vector2 reflectedVelocity = Vector3.Reflect(_lastVelocity, contact.normal);
                // assign the reflected velocity back to the rigidbody
                _rigidBody.velocity = reflectedVelocity;
                // rotate the object by the same ammount we changed its velocity
                Quaternion rotation = Quaternion.FromToRotation(_lastVelocity, reflectedVelocity);
                transform.rotation = rotation * transform.rotation;
                // play bounce sound
                AudioManager.Instance.PlayShootingSFX("Bullet Bounce Sound");
            } else {

                // do nothing?


            }
        }




    }


    public void SetVelocity(float velocity) {
        _velocity = velocity;
    }

    public void SetDamage(int damage) {
        _damage = damage;
    }

    public void SetCritRate(float critRate) {
        _critRate = critRate;
    }

    public void SetPierceCount(int pierceCount) {
        _pierceLimit = pierceCount;
    }

    public void SetBounce(bool bounce) {
        _reflect = bounce;
    }




}
