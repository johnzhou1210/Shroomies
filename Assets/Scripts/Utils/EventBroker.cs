using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventBroker : MonoBehaviour
{
    public static event Action<Palette> onPaletteChange;

    public static void CallPaletteChange(Palette currentPalette)
    {
        onPaletteChange?.Invoke(currentPalette);
    }
}
