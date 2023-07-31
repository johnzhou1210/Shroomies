using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* CONTEXT */
public class PlayerStateManager : MonoBehaviour
{
    public PlayerBaseState CurrentState;
    public PlayerAlive AliveState = new PlayerAlive();
    public PlayerDead DeadState = new PlayerDead();
    
    private void Start() {
        // starting state for state machine
        CurrentState = AliveState;
        CurrentState.EnterState(this);
    }
    
    private void Update() {
        CurrentState.UpdateState(this);
    }
    
    public void SwitchState(PlayerBaseState state) {
        CurrentState = state;
        state.EnterState(this);
    }

    public void OnDeath() {
        SwitchState(DeadState);
    }

}
