using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : PlayerBaseState {
    public override void EnterState(PlayerStateManager mgr) {
        Debug.Log("Enter PlayerDead state");
        // get player animator
        Animator animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        animator.Play("PlayerDeath");
        
    }

    public override void UpdateState(PlayerStateManager mgr) {
        // called every frame
    }
}
