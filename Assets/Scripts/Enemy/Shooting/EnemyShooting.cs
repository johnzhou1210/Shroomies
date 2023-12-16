using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemyShooting : MonoBehaviour {
    [Range(.01f, 16f)] public float FireRate = .25f;
    [Range(0, 8f)] public float BulletVelocity = 5f;
    [Range(0, 64)] public int AttackPower = 1;
    public float StartShootY = 5.5f;
    public float EndShootY = -3f;
    public bool BulletsBounce = false;
    public int ObstaclePierceCount = 0, BulletClearLimit = 0;

    public BulletType CurrentBulletType;

    public Animator Animator;
    public EnemyStateManager StateManager;
    public BarrelConfiguration CurrentBarrelConfiguration;
    public BarrelConfiguration[] BarrelConfigurations;

    private void Start() {
        Animator = GetComponent<Animator>();
        StateManager = GetComponent<EnemyStateManager>();
        if (BarrelConfigurations.Length > 0) {
            CurrentBarrelConfiguration = BarrelConfigurations[0];
        } else {
            Debug.LogError(transform.name + " does not have any set barrel configuration!");
        }
        ExecuteAI();
    }

    public void ExecuteAI() {
        StartCoroutine(monsterBehavior());
    }

    IEnumerator monsterBehavior() {

        // Should the game require the player to defeat all the enemies in a wave to progress?
        // basic flower: stationary. Attack

        // Flower will just loop basic bullet attack every 3-4 seconds
        while (StateManager.CurrentState != StateManager.DeadState) {
            Debug.Log("in loop. current state is " + StateManager.CurrentState);
            yield return new WaitForSeconds(FireRate / 2f);

            if (transform.position.y <= StartShootY && transform.position.y >= EndShootY) {
                Animator.speed = Mathf.Clamp(1 / FireRate, 1f, 16f);
                Animator.Play("FlowerShoot");
            }
 
            yield return new WaitForSeconds(FireRate / 2f);

        }
        Debug.Log(" out of loop ");
        yield return null;
    }

    public void DefaultShootSound() {
        AudioManager.Instance.PlaySFX("Enemy Shoot Sound");
    }

    public void OnDeath() {
        StopAllCoroutines();
    }

    public void Fire() {
        StartCoroutine(fire());
    }

    IEnumerator fire() {
        // shoot bullets depending on current barrel configuration.
        BulletDamageInfo dmgInfo = new BulletDamageInfo(BulletVelocity, AttackPower, 0, ObstaclePierceCount, BulletsBounce, BulletClearLimit);
        CurrentBarrelConfiguration.Fire(CurrentBulletType, BulletOwnershipType.ENEMY, dmgInfo);
        Debug.Log("Shot enemy bullet");
        yield return null;
    }

}
