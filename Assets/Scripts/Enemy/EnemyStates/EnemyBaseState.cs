using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyStateManager mgr);

    public abstract void UpdateState(EnemyStateManager mgr);
}
