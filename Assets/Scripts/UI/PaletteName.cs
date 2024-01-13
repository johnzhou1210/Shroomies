using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaletteName : MonoBehaviour
{
    public GameSettings gameSettings;

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TMP_Text>().text = gameSettings.paletteName;
    }
}
