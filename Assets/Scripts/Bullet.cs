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
    int _damage = 1, _pierceLimit = 32, _bulletClearLimit = 0;
    public Transform Shooter;
    public BulletType Type;
    public BulletOwnershipType Ownership;
    public HashSet<IDamageable> HitTargets;
    [SerializeField] GameObject _explosionPrefab, _critEffect;

    readonly float _removeTime = 9f;
    int _pierceCounter = 0, _bulletClearCounter = 0;

    [SerializeField] Rigidbody2D _rigidBody;

    Vector2 _lastVelocity = Vector2.zero;

    bool _debounce = false;

    private void OnEnable() {
        Invoke("Destroy", _removeTime);
        HitTargets = new HashSet<IDamageable>();
        _pierceCounter = 0;
        _bulletClearCounter = 0;
    }

    private void FixedUpdate() {
        //transform.Translate(Velocity * Time.deltaTime * _moveDir);
        //_lastFramePos = transform.position;
        _lastVelocity = _rigidBody.velocity;
        if (transform.position.y < -6 || (Ownership == BulletOwnershipType.PLAYER && transform.position.y > 5.4f)) {
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


    bool gameObjectIsABullet(GameObject go) {
        return (go.layer == 7 || go.layer == 9);
    }
    bool bulletCanClearBullets(Bullet b) {
        return b._bulletClearLimit > 0 &&
               b._bulletClearCounter < b._bulletClearLimit;
    }


    void OnCollisionEnter2D(Collision2D collision) {
        GameObject hitTarget = collision.gameObject;



        void bounce() {
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
        }

        void ignoreCollisionsWithTarget() {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);
        }
        void keepBulletGoingAfterCollision() {
            _rigidBody.velocity = _lastVelocity;
        }


        if (transform.position.y < 5.1f) {


            if (((hitTarget.CompareTag("Enemy") && Ownership == BulletOwnershipType.ENEMY) || (hitTarget.CompareTag("Player") && Ownership == BulletOwnershipType.PLAYER))
                  ) {
                if (hitTarget.layer == 11) {
                    Debug.Log("call 1 " + transform.name + " collision enter with " + hitTarget.name);
                    Destroy();
                }
            } else {
                if (_debounce == false) {
                    // play hit sound
                    _debounce = true;
                }
                //Debug.Log(transform.name + " collision enter with " + hitTarget.name);
                if (hitTarget.CompareTag("Player") || hitTarget.CompareTag("Enemy")) {
                    IDamageable onHit;
                    if (hitTarget.CompareTag("Player")) {
                        onHit = hitTarget.gameObject.GetComponent<PlayerOnHit>();
                    } else {
                        bool hasComponent = (hitTarget.TryGetComponent(out EnemyOnHit eoh));
                        if (hasComponent) {
                            onHit = eoh;
                        } else {
                            // must be in parent or else error
                            onHit = hitTarget.transform.parent.gameObject.GetComponent<EnemyOnHit>();
                        }
                    }
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
                        Debug.Log("call 2 " + transform.name + " collision enter with " + hitTarget.name);
                        Destroy();
                    } else {
                        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                        _pierceCounter++;
                        // ignore collisions with this target from now on
                        ignoreCollisionsWithTarget();
                        keepBulletGoingAfterCollision();
                    }

                } else if (hitTarget.CompareTag("Obstacle")) {
                    // hit obstacle
                    Debug.Log("hit obstacle");
                    if (_pierceLimit > 0 && _pierceCounter < _pierceLimit) { // pierce check
                        Debug.Log("pierced obstacle");
                        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                        _pierceCounter++;
                        // ignore collisions with this target from now on
                        ignoreCollisionsWithTarget();
                        keepBulletGoingAfterCollision();
                    } else if (_reflect) { // bounce check
                        bounce();
                    } else {
                        Debug.Log("call 3 " + transform.name + " collision enter with " + hitTarget.name);
                        Destroy();
                    }

                } else if (_reflect && !hitTarget.CompareTag("Player") && !hitTarget.CompareTag("Enemy")) {
                    // bounce off contact point
                    bounce();
                } else {
                    // if this is a clearing bullet
                    if (bulletCanClearBullets(this) && gameObjectIsABullet(hitTarget) && Shooter != hitTarget.GetComponent<Bullet>().Shooter) { 
                        //&& !bulletCanClearBullets(hitTarget.GetComponent<Bullet>())) {
                        // destroy other bullet
                 
                        hitTarget.gameObject.GetComponent<Bullet>().Destroy();
                        keepBulletGoingAfterCollision();
                        _bulletClearCounter++;
                        if (_bulletClearCounter >= _bulletClearLimit) {
                            Debug.Log("call 4 " + transform.name + " collision enter with " + hitTarget.name);
                            Destroy();
                        }
                    } else {
                        if (!bulletCanClearBullets(this) || gameObjectIsABullet(hitTarget)) {
                            // just "pierce" it
                            // ignore collisions with this target from now on
                            Debug.Log("ignoring collisions involving " + transform.name + " with " + hitTarget.name);
                            ignoreCollisionsWithTarget();
                            keepBulletGoingAfterCollision();
                        } else {
                            Debug.Log("call 5 " + transform.name + " collision enter with " + hitTarget.name);
                            Destroy();
                        }

                    }
                }
            }
        } else {
            // ignore collisions with this target from now on
            ignoreCollisionsWithTarget();
            keepBulletGoingAfterCollision();
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

    public void SetBulletClearLimit(int limit) {
        _bulletClearLimit = limit;
    }

    public void SetShooter(Transform shooter) {
        Shooter = shooter;
    }




}
