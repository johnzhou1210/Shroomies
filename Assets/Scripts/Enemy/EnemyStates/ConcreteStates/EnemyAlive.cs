using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlive : EnemyBaseState
{

    public override void EnterState(EnemyStateManager mgr) {
        Debug.Log("Enter EnemyAlive state");
    }

    public override void UpdateState(EnemyStateManager mgr) {
        // called every frame
        // to change state:
        //mgr.SwitchState(mgr.DeadState);
    }

    

}
