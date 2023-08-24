using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class DaisyShooting : EnemyShooting {

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

    public new void ExecuteAI() {
        StartCoroutine(monsterBehavior());
    }

    IEnumerator monsterBehavior() {
        Animator.speed = Mathf.Clamp(1 / FireRate, 1f, 16f);
        while (StateManager.CurrentState != StateManager.DeadState) {
            //Debug.Log("in loop. current state is " + StateManager.CurrentState);
            yield return new WaitForSeconds(FireRate / 2f);
            if (transform.position.y <= StartShootY) {
                CurrentBarrelConfiguration = BarrelConfigurations[0];
                Animator.Play("DaisyFireY");
            }
            
            yield return new WaitForSeconds(FireRate);
            if (transform.position.y <= StartShootY) {
                CurrentBarrelConfiguration = BarrelConfigurations[1];
                Animator.Play("DaisyFireY2");
            }
            yield return new WaitForSeconds(FireRate / 2f);
        }
        Debug.Log(" out of loop ");
        yield return null;
    }


}
