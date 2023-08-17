using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PetuniaShooting : EnemyShooting {

    private void Start() {
        Animator = GetComponent<Animator>();
        StateManager = GetComponent<EnemyStateManager>(); 
        ExecuteAI();
    }

    public new void ExecuteAI() {
        StartCoroutine(monsterBehavior());
    }

    IEnumerator monsterBehavior() {
        while (StateManager.CurrentState != StateManager.DeadState) {
            Debug.Log("in loop. current state is " + StateManager.CurrentState);
            yield return new WaitForSeconds(FireRate / 2f);

            if (transform.position.y <=StartShootY) {
                Animator.speed = Mathf.Clamp(1 / FireRate, 1f, 16f);
                Animator.Play("PetuniaShoot");
            }
 
            yield return new WaitForSeconds(FireRate / 2f);

        }
        Debug.Log(" out of loop ");
        yield return null;
    }


    IEnumerator fire() {
        // shoot bullets depending on current barrel configuration.
        BulletDamageInfo dmgInfo = new BulletDamageInfo(BulletVelocity, AttackPower, 0, ObstaclePierceCount, BulletsBounce);
        BarrelConfiguration.Fire(CurrentBulletType, BulletOwnershipType.ENEMY, dmgInfo);
        Debug.Log("Shot enemy bullet");
        yield return null;
    }

}
