using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckBox : MonoBehaviour
{

    [SerializeField] bool random = true;

    public void Xmark() {
        if (!random) {
            gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "X";
            AudioManager.Instance.PlaySFX("UI Select Sound");
            random = true;
        }
        else if(random) {
            gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = " ";
            AudioManager.Instance.PlaySFX("UI Select Sound");
            random = false;
        }
    }
}
