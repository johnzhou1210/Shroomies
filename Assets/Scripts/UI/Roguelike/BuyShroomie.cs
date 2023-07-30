using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyShroomie : MonoBehaviour
{
    StageLogic _roguelikeManager;
    [SerializeField] GameObject _priceText;
    int shroomieCost = -1;

    private void Start() {
        _roguelikeManager = GameObject.FindWithTag("Roguelike Manager").gameObject.GetComponent<StageLogic>();
    }

    public void OnClick() {
        if (shroomieCost > 0) {
            bool enoughMulch = _roguelikeManager.AccumulatedMulch >= shroomieCost;
             
            if (enoughMulch) {
                AudioManager.Instance.PlaySFX("UI Select Sound");
                _roguelikeManager.decreaseMulch(shroomieCost);
            } else {
                AudioManager.Instance.PlaySFX("Deny Sound");
            }

            
        }
    }

    public void OnChangePrice(int newPrice) {
        shroomieCost = newPrice;
        _priceText.GetComponent<TextMeshProUGUI>().text = "-" + newPrice.ToString();
    }

    public void onFinishShroomieButtonHide() {
        gameObject.SetActive(false);
    }

}
