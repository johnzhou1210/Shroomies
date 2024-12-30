using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyShroomie : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    StageLogic _roguelikeManager;
    [SerializeField] GameObject _priceText;
    [SerializeField] Sprite _pressedSprite, _defaultSprite, _buySprite;
    int shroomieCost = -1;
    bool enoughMulch;
    public float closeEnoughX, closeEnoughY;

    private void Start() {
        playerInputActions = InputManager.InputActions;
        _roguelikeManager = GameObject.FindWithTag("Roguelike Manager").gameObject.GetComponent<StageLogic>();
        shroomieCost = _roguelikeManager.ShroomieBaseCost;
    }

    private void OnEnable()
    {
        StageLogic.OnShroomieUpdateCost += UpdateShroomiePrice;
    }

    private void OnDisable()
    {
        StageLogic.OnShroomieUpdateCost -= UpdateShroomiePrice;
    }

    private void Update() {
        enoughMulch = _roguelikeManager.AccumulatedMulch >= shroomieCost;

        if (playerInputActions.Player.Shroomies.WasReleasedThisFrame()) {
            OnClick();
        }
        if (playerInputActions.Player.Shroomies.WasPressedThisFrame()) {
            transform.parent.GetComponent<Image>().sprite = _pressedSprite;
        }
        if (enoughMulch && transform.parent.GetComponent<Image>().sprite != _pressedSprite) {
            transform.parent.GetComponent<Image>().sprite = _buySprite;
        } else if (transform.parent.GetComponent<Image>().sprite != _pressedSprite) {
            transform.parent.GetComponent<Image>().sprite = _defaultSprite;
        }

    }

    public bool CloseEnough(float a, float b) {
        Debug.Log(Mathf.Abs(a - b) < 10f);
        return Mathf.Abs(a - b) < 10f;
    }

    public void OnClick() {
        if (CloseEnough(transform.parent.GetComponent<RectTransform>().anchoredPosition.x, closeEnoughX) && CloseEnough(transform.parent.GetComponent<RectTransform>().anchoredPosition.y, -closeEnoughY)) {
            if (shroomieCost > 0) {
                int currNumShroomies = GameObject.FindWithTag("Player").GetComponent<PlayerOnHit>().CurrentShroomies;
                ShroomieFormation formation = GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomieFormation>();
                if (enoughMulch && currNumShroomies < formation.ShroomieObjects.Count) {
                    AudioManager.Instance.PlaySFX("UI Select Sound");
                    _roguelikeManager.incrementAccumulatedShroomies();
                    _roguelikeManager.decreaseMulch(shroomieCost);
                    // make future purchases more expensive
                    UpdateShroomiePrice((int)(shroomieCost + ((_roguelikeManager.AccumulatedShroomies + 1) * 25)));
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
        transform.parent.GetComponent<Image>().sprite = _defaultSprite;
    }

    public void OnButtonDown() {
        if (_priceText != null)
            _priceText.GetComponent<TextMeshProUGUI>().enabled = false;
    }


    public void OnButtonUp() {
        if (_priceText != null)
            _priceText.GetComponent<TextMeshProUGUI>().enabled = true;
    }

    public void UpdateShroomiePrice(int newPrice) {
        if (_priceText != null) {
            shroomieCost = newPrice;
            _priceText.GetComponent<TextMeshProUGUI>().text = "" + newPrice.ToString();
        }
    }

    public void OnFinishShroomieButtonHide() {
        gameObject.SetActive(false);
    }

}
