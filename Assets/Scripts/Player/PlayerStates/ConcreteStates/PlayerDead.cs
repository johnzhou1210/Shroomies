using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : PlayerBaseState {
    public override void EnterState(PlayerStateManager mgr) {
        Debug.Log("Enter PlayerDead state");
    }

    public override void UpdateState(PlayerStateManager mgr) {
        // called every frame
    }
}
