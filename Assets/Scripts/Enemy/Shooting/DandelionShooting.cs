using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class DandelionShooting : EnemyShooting {

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

    public void SelfDestruct() {
        Animator.Play("DandelionSelfDestruct");
    }

    IEnumerator monsterBehavior() {
        yield return null;
    }

}
