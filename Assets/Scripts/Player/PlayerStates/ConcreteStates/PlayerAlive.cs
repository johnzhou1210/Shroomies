using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlive : PlayerBaseState {
    public override void EnterState(PlayerStateManager mgr) {
        Debug.Log("Enter PlayerAlive state");
    }

    public override void UpdateState(PlayerStateManager mgr) {
        // called every frame
        // to change state:
        // mgr.SwitchState(mgr.DeadState)
    }
}
