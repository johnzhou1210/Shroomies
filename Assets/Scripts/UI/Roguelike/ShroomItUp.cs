using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ShroomItUp : MonoBehaviour
{
    StageLogic _roguelikeManager;

    [SerializeField] GameObject _checkBox, _costField;
    [SerializeField] UnityBoolEvent _updateUpgradeDescriptionToShroomies;
    public int ShroomItUpCost = 0;
    public bool CheckBoxSelected = false;

    private void Start()
    {
        _roguelikeManager = GameObject.FindWithTag("Roguelike Manager").gameObject.GetComponent<StageLogic>();
    }

    public void OnUpdateShroomItUpCost(int newVal) {
        ShroomItUpCost = newVal;
    }

    public void OnCheckboxClick() {
        
        if (CheckBoxSelected) {
            CheckBoxSelected = false;
            _checkBox.transform.Find("X").GetComponent<TextMeshProUGUI>().text = "";
            AudioManager.Instance.PlaySFX("UI Select Sound");
            StartCoroutine(SelectAnim());
            _updateUpgradeDescriptionToShroomies.Invoke(false);
        }
        else
        {
            bool enoughMulch = GameObject.FindWithTag("Roguelike Manager").gameObject.GetComponent<StageLogic>().AccumulatedMulch >= ShroomItUpCost;
            if (enoughMulch) {
                CheckBoxSelected = true;
                _checkBox.transform.Find("X").GetComponent<TextMeshProUGUI>().text = "X";
                AudioManager.Instance.PlaySFX("UI Select Sound");
                StartCoroutine(SelectAnim());
                _updateUpgradeDescriptionToShroomies.Invoke(true);
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
        ShroomItUpCost = (int)(Mathf.Pow(GameObject.FindWithTag("Roguelike Manager").GetComponent<StageLogic>().StageNumber * 20, 1.45f) + 25);

        //ShroomItUpCost = (int)(ShroomItUpCost + ((_roguelikeManager.AccumulatedShroomies + 1) * 50));
        //ShroomItUpCost = (int)(ShroomItUpCost + ((_roguelikeManager.AccumulatedShroomItUps + 1) * 50));

        //ShroomItUpCost = (int)(GameObject.FindWithTag("Roguelike Manager").GetComponent<StageLogic>().StageNumber * 20);

        if (ShroomItUpCost % 5 != 0)
        {
            ShroomItUpCost = (int)(Mathf.Floor(ShroomItUpCost / 5) * 5);
        }

        if (ShroomItUpCost > 100)
        {
            ShroomItUpCost -= 25;
        }

        _costField.GetComponent<TextMeshProUGUI>().text = "Shroom it up?\n\n" + "<color=#" + ChangePalette.holder.color2.ToHexString() + ">" + ShroomItUpCost.ToString() + "</color>";
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
