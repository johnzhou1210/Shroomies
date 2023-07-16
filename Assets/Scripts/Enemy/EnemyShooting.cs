using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemyShooting : MonoBehaviour {
    
    [Range(.01f, 5f)] [SerializeField]  float FireRate = .25f;
    [Range(0, 8f)] [SerializeField] float BulletVelocity = 5f;
    [Range(0, 64)] [SerializeField] int AttackPower = 1;

    public GameObject bullet;

    Animator _animator;
    EnemyStateManager _stateManager;
    [SerializeField] BarrelConfiguration _barrelConfiguration;

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
            Debug.Log("in loop. current state is " + _stateManager.CurrentState);
            yield return new WaitForSeconds(FireRate / 2f);

            if (transform.position.y <= 5.5f) {
                _animator.speed = Mathf.Clamp(1 / FireRate, 1f, 16f);
                _animator.Play("FlowerShoot");
            }
 
            yield return new WaitForSeconds(FireRate / 2f);

        }
        Debug.Log(" out of loop ");
        yield return null;
    }

    public void OnDeath() {
        _animator.Play("Dead");
    }

    public void Fire() {
        StartCoroutine(fire());
    }

    IEnumerator fire() {
        // shoot bullets depending on current barrel configuration.
        _barrelConfiguration.Fire(BulletType.NORMAL, BulletOwnershipType.ENEMY, BulletVelocity, AttackPower);
        Debug.Log("Shot enemy bullet");
        yield return null;
    }
}
