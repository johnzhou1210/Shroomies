using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class GameSettings : ScriptableObject {

    public int currentTrack;
    public int currentPalette;
    public string paletteName;
}
