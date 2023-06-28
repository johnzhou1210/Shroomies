using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour
{
    bool _toggle = false;
    float _cooldown = 1f;
    public float _fireRate = .25f;

    public GameObject bullet;


    private void Start() {
        PlayerEventManager.Instance.OnSingleTap += onSingleTap;
        PlayerEventManager.Instance.OnDoubleTap += onDoubleTap;
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
        GetComponent<Animator>().Play("PlayerShoot");
        yield return null;
    }

    public void Fire() {
        StartCoroutine(fire());
    }

    IEnumerator fire() {
        Debug.Log("Shot bullet");
        GetComponent<AudioSource>().Play();
        Debug.Log("in here2");
        Debug.Log(BulletPool.BulletPoolInstance);
        BulletInfo newBulletInfo = BulletPool.BulletPoolInstance.GetBullet(BulletType.NORMAL, BulletOwnershipType.PLAYER, 5f, 1);
        GameObject newBullet = newBulletInfo.reference;
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = Quaternion.identity;
        //GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity, GameObject.FindWithTag("PlayArea").transform.Find("Bullet Pool").transform);
        newBullet.GetComponent<Bullet>().SetMoveDirection(Vector2.up);
        yield return null;
    }


}
