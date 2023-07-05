using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyBaseState CurrentState;
    public EnemyAlive AliveState = new EnemyAlive();
    public EnemyDead DeadState = new EnemyDead();

    private void Start() {
        // starting state for state machine
        CurrentState = AliveState;
        CurrentState.EnterState(this);
    }

    private void Update() {
        CurrentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state) {
        CurrentState = state;
        state.EnterState(this);
    }

    public void onDeath() {
        SwitchState(DeadState);
    }


}
