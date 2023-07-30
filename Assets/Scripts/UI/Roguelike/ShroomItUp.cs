using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShroomItUp : MonoBehaviour
{
    [SerializeField] GameObject _checkBox;
    public int ShroomItUpCost = 500;
    public bool CheckBoxSelected = false;

    public void OnCheckboxClick() {
        
        if (CheckBoxSelected) {
            CheckBoxSelected = false;
            _checkBox.transform.Find("X").GetComponent<TextMeshProUGUI>().text = "";
            AudioManager.Instance.PlaySFX("UI Select Sound");
            StartCoroutine(SelectAnim());
        }
        else
        {
            bool enoughMulch = GameObject.FindWithTag("Roguelike Manager").gameObject.GetComponent<StageLogic>().AccumulatedMulch >= ShroomItUpCost;
            if (enoughMulch) {
                CheckBoxSelected = true;
                _checkBox.transform.Find("X").GetComponent<TextMeshProUGUI>().text = "X";
                AudioManager.Instance.PlaySFX("UI Select Sound");
                StartCoroutine(SelectAnim());
            }
            else
            {
                AudioManager.Instance.PlaySFX("Deny Sound");
                StartCoroutine(DenyAnim());
            }
        }
    }

    private void OnEnable() {
        if (CheckBoxSelected) {
            CheckBoxSelected = false;
            _checkBox.transform.Find("X").GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    IEnumerator SelectAnim() {
        for (float i = 0; i < Mathf.PI * 2; i += (Mathf.PI / 16f)) {
            _checkBox.transform.parent.parent.localScale += new Vector3(Mathf.Sin(i) / 64f, Mathf.Sin(i) / 64f, 0);
            yield return new WaitForSeconds(.01f);
        }
        yield return null;
    }

    IEnumerator DenyAnim() {
        for (float i = 0; i < Mathf.PI * 4; i += (Mathf.PI / 4f)) {
            _checkBox.transform.parent.parent.localPosition += new Vector3(Mathf.Sin(i) * 8f, 0, 0);
            yield return new WaitForSeconds(.01f);
        }
        yield return null;
    }

}
