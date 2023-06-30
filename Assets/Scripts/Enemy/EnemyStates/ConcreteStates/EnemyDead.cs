using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : EnemyBaseState
{
    public override void EnterState(EnemyStateManager mgr) {
        Debug.Log("Enter EnemyDead state");
    }

    public override void UpdateState(EnemyStateManager mgr) {
        // called every frame
    }
}
