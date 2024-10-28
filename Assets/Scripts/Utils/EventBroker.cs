using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventBroker : MonoBehaviour
{
    public static event Action OnPaletteChange;

    public static void CallPaletteChange()
    {
        OnPaletteChange?.Invoke();
    }
}
