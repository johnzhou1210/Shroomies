using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ShroomSnailAI : EnemyShooting {
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

    /*
     * SNAIL BOSS AI
     * - CONSISTS OF 4 MOVES:
     *      - WIDE SWEEP
     *          - Play wide sweep animation
     *          - Attach sweep attack events to each attack moment
     *          - Sweeping bullets destroy both player and enemy bullets.
     *      - NECK ATTACK
     *          - Play neck attack animation
     *          - Make boss' neck harmful
     *          - Make boss' sprite invulnerable but neck vulnerable
     *      - TRIPLE SHOT
     *          - Play triple shot animation
     *          - Attach triple attack event to attack moment
     *      - ROLLOUT
     *          - Play prepare animation
     *              - Attach event to end of prepare animation to play one of the 3 types of rollouts as listed below.
     *          - Make rolling shell harmful
     *          - Make boss invulnerable
     *          - 3 Types (refer to sketch):
     *              1) Spiral
     *              2) Zig-zag
     *              3) Looping
     * - ATTACK MOVE INTERVAL DECREASES AS HEALTH DECREASES.
     * - MINIONS SPAWN WHEN BOSS IS <= 50% HP.
     */

    public new void ExecuteAI() {
        StartCoroutine(monsterBehavior());
    }

    IEnumerator monsterBehavior() {
        yield return new WaitForSeconds(5f);
        while (StateManager.CurrentState != StateManager.DeadState) {
            //Debug.Log("in loop. current state is " + StateManager.CurrentState);
            yield return new WaitForSeconds(FireRate / 2f);

            if (transform.position.y <= StartShootY) {
                Animator.speed = Mathf.Clamp(1 / FireRate, 1f, 16f);
                //Animator.Play("PetuniaShoot");
            }

            yield return new WaitForSeconds(FireRate / 2f);

        }
        Debug.Log(" out of loop ");
        yield return null;
    }

}
