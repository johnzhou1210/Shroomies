using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyShroomie : MonoBehaviour
{
    StageLogic _roguelikeManager;
    [SerializeField] GameObject _priceText;
    int shroomieCost = -1;

    private void Start() {
        _roguelikeManager = GameObject.FindWithTag("Roguelike Manager").gameObject.GetComponent<StageLogic>();
    }

    public void OnClick() {
        Debug.Log("clicked button");
        if (shroomieCost > 0) {
            bool enoughMulch = _roguelikeManager.AccumulatedMulch >= shroomieCost;
            int currNumShroomies = GameObject.FindWithTag("Player").GetComponent<PlayerOnHit>().CurrentShroomies;
            ShroomieFormation formation = GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomieFormation>();
            if (enoughMulch && currNumShroomies < formation.ShroomieObjects.Count) {
                AudioManager.Instance.PlaySFX("UI Select Sound");
                _roguelikeManager.decreaseMulch(shroomieCost);
                // make future purchases more expensive
                //OnChangePrice((int)(shroomieCost * 2f));
                // add a shroomie
                GameObject.FindWithTag("Player").GetComponent<PlayerOnHit>().CurrentShroomies++;
                currNumShroomies = GameObject.FindWithTag("Player").GetComponent<PlayerOnHit>().CurrentShroomies;
                GameObject shroomieToEnable = formation.ShroomieObjects.Find(obj => obj.name == currNumShroomies.ToString());
                shroomieToEnable.SetActive(true);
            } else {
                AudioManager.Instance.PlaySFX("Deny Sound");
            }

            
        }
    }

    public void OnButtonDown() {
        _priceText.GetComponent<TextMeshProUGUI>().enabled = false;
    }

    public void OnButtonUp() {
        _priceText.GetComponent<TextMeshProUGUI>().enabled = true;
    }

    public void OnChangePrice(int newPrice) {
        shroomieCost = newPrice;
        _priceText.GetComponent<TextMeshProUGUI>().text = "" + newPrice.ToString();
    }

    public void onFinishShroomieButtonHide() {
        gameObject.SetActive(false);
    }

}
