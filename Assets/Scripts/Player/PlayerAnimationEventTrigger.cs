using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEventTrigger : MonoBehaviour
{
    public UnityEvent _onShootBegin;

    public void invokeFire() {
        _onShootBegin.Invoke();
    }

}
