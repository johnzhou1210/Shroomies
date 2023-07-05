using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[System.Serializable]
public class UnityIntEvent : UnityEvent<int> { }

public class EnemyShooting : MonoBehaviour {
    [Range(.01f, 5f)]
    public float _fireRate = .25f;

    public GameObject bullet;

    Animator _animator;
    EnemyStateManager _stateManager;

    private void Start() {
        _animator = GetComponent<Animator>();
        _stateManager = GetComponent<EnemyStateManager>(); 
        ExecuteAI();
    }

    public void ExecuteAI() {
        StartCoroutine(monsterBehavior());
    }

    IEnumerator monsterBehavior() {
        // Should the game require the player to defeat all the enemies in a wave to progress?
        // basic flower: stationary. Attack

        // Flower will just loop basic bullet attack every 3-4 seconds
        while (_stateManager.CurrentState != _stateManager.DeadState) {
            yield return new WaitForSeconds(_fireRate / 2f);
            _animator.speed = Mathf.Clamp(1 / _fireRate, 1f, 16f);
            _animator.Play("FlowerShoot");
            yield return new WaitForSeconds(_fireRate / 2f);

        }

        yield return null;
    }

    private void Update() {

    }



    public void Fire() {
        StartCoroutine(fire());
    }

    IEnumerator fire() {
        Debug.Log("Shot enemy bullet");
        AudioManager.Instance.PlaySFX("Enemy Shoot Sound");
        Debug.Log(BulletPool.BulletPoolInstance);
        BulletInfo newBulletInfo = BulletPool.BulletPoolInstance.GetBullet(BulletType.NORMAL, BulletOwnershipType.ENEMY, 5f, 1);
        GameObject newBullet = newBulletInfo.Reference;
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = Quaternion.identity;
        //GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity, GameObject.FindWithTag("PlayArea").transform.Find("Bullet Pool").transform);
        newBullet.GetComponent<Bullet>().SetMoveDirection(Vector2.down);
        yield return null;
    }
}
