using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RenderUpgradeButton : MonoBehaviour
{
    public Upgrade Upgrade;


    // Update is called once per frame
    void Update()
    {
        if (Upgrade != null) {
            GetComponent<Image>().sprite = Upgrade.Image;
        }
        
    }
}
