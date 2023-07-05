using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerStateManager))]
public class PlayerShooting : MonoBehaviour
{
    bool _toggle = false;
    float _cooldown = 1f;

    [Range(.01f,5f)]
    public float _fireRate = .25f;

    public GameObject bullet;

    Animator _animator;
    PlayerStateManager _currentState;

    private void Start() {
        PlayerTapHandler.Instance.OnSingleTap += onSingleTap;
        PlayerTapHandler.Instance.OnDoubleTap += onDoubleTap;
        _animator = GetComponent<Animator>();
        _currentState = GetComponent<PlayerStateManager>();
    }

    private void OnDisable() {
        StopAllCoroutines();
        // also deactivate all bullets in pool?
    }

    void onSingleTap() {

    }

    void onDoubleTap() {
        Debug.Log("In here");
        _toggle = !_toggle;
    }

    private void Update() {
        _cooldown = Mathf.Clamp(_cooldown - Time.deltaTime, 0, _fireRate);
        if (_toggle && _cooldown <= 0f) {
            // shoot bullet
            StartCoroutine(fireAnim());
            // reset cooldown
            _cooldown = _fireRate;
        }
    }

    IEnumerator fireAnim() {
        // play animation
        _animator.speed = 1 / _fireRate;
        _animator.Play("PlayerShoot");
        yield return null;
    }

    public void Fire() {
        StartCoroutine(fire());
    }

    IEnumerator fire() {
        Debug.Log("Shot bullet");
        AudioManager.Instance.PlaySFX("Player Shoot Sound");
        Debug.Log("in here2");
        Debug.Log(BulletPool.BulletPoolInstance);
        BulletInfo newBulletInfo = BulletPool.BulletPoolInstance.GetBullet(BulletType.NORMAL, BulletOwnershipType.PLAYER, 5f, 1);
        GameObject newBullet = newBulletInfo.Reference;
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = Quaternion.identity;
        //GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity, GameObject.FindWithTag("PlayArea").transform.Find("Bullet Pool").transform);
        newBullet.GetComponent<Bullet>().SetMoveDirection(Vector2.up);
        yield return null;
    }


}
