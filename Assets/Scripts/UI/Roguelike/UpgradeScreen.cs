using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UpgradeScreen : MonoBehaviour
{
    Animator _animator;
    bool _selected = false;
    GameObject SelectedButton = null;
    [SerializeField] GameObject _title, _buttons, _desc, _upgradeName, _upgradeBack, _shroomItUp;
    [SerializeField] GameObject _contents;
    public bool Upgrading = false;

    public UnityUpgradeBoolEvent onPlayerUpgrade;


    private void OnEnable() {
        _animator = GetComponent<Animator>();
        onShow();
    }

    private void Start() {
        onPlayerUpgrade.AddListener(GameObject.FindWithTag("UpgradeManager").GetComponent<UpgradeManager>().OnUpgrade); 
    }

    public void ShowShroomiesDescription(bool val) {
        if (val) {
            setDescription(SelectedButton.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade.ShroomItUpDescription);
        } else {
            setDescription(SelectedButton.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade.UpgradeDescription);
        }
        
    }

    public void onClick(GameObject buttonObj) {
        _upgradeBack.SetActive(true);
        if  (_selected && SelectedButton == buttonObj) {
            // hide description and title
            setDescription("");
            setTitle("");
            setUpgradeName("");
            setShroomItUpVisibility(false);
            _upgradeBack.SetActive(false);
            foreach (Transform child in _buttons.transform) {
                child.gameObject.GetComponent<Button>().enabled = false;
                if (child.gameObject != buttonObj) {
                    child.gameObject.GetComponent<Image>().enabled = false;
                    child.gameObject.GetComponent<Animator>().Play("UpgradeButtonInvisible");
                    child.gameObject.transform.Find("Image").GetComponent<Image>().enabled = false;
                }
            }
            // confirm selection
            // check if shroom it up is selected. if so, charge mulch.
            if (_shroomItUp.GetComponent<ShroomItUp>().CheckBoxSelected) {
                GameObject.FindWithTag("Roguelike Manager").GetComponent<StageLogic>().decreaseMulch(_shroomItUp.GetComponent<ShroomItUp>().ShroomItUpCost);
            }
            // play confirm animation on buttonObj
            AudioManager.Instance.PlaySFX("Player Get Upgrade");
            buttonObj.GetComponent<Animator>().Play("UpgradeButtonConfirmed");
            GetComponent<Animator>().Play("UpgradeFrameFadeOut");
            // fire player upgrade event
            onPlayerUpgrade.Invoke(buttonObj.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade, _shroomItUp.GetComponent<ShroomItUp>().CheckBoxSelected);

            SelectedButton = null;
        } else {
            if (_selected && SelectedButton != buttonObj) {
                // deselect previous selected button
                SelectedButton.GetComponent<Animator>().Play("UpgradeButtonUnselected");
            }
            // choose selection
            SelectedButton = buttonObj;
            // update description depending on whether checkbox is checked.
            ShowShroomiesDescription(_shroomItUp.GetComponent<ShroomItUp>().CheckBoxSelected);
            _selected = true;
            setTitle("Tap again to confirm.");
            setUpgradeName(buttonObj.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade.UpgradeName);
            //setDescription(buttonObj.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade.UpgradeDescription);
            setShroomItUpVisibility(true);
            // play selected animation on selectedButton
            buttonObj.GetComponent<Animator>().Play("UpgradeButtonSelect");

        }
    }

    void setDescription(string description) {
        _desc.GetComponent<TextMeshProUGUI>().text = description;
    }

    void setTitle(string title) {
        _title.GetComponent<TextMeshProUGUI>().text = title;
    }

    void setUpgradeName(string name) {
        _upgradeName.GetComponent<TextMeshProUGUI>().text = name;
    }

    void setShroomItUpVisibility(bool val) {
        _shroomItUp.SetActive(val);
    }

    public void showContents() {
        _contents.SetActive(true);
        foreach (Transform child in _buttons.transform) {
            child.gameObject.GetComponent<Button>().enabled = true;
            child.gameObject.GetComponent<Image>().enabled = true;
            child.gameObject.GetComponent<Animator>().Play("UpgradeButtonUnselected");
        }
    }

    public void hideContents() {
        _contents.SetActive(false);
        _selected = false; SelectedButton = null;
        gameObject.SetActive(false);
    }

    public void onShow() {
        Debug.Log("onshow called");
        setDescription("Tap on an upgrade to view its description.");
        setTitle("Choose an upgrade!");
        setShroomItUpVisibility(false);
        
        GetComponent<Animator>().Play("UpgradeFrameFadeIn");

        StartCoroutine(GenerateOptions());

    }

    IEnumerator GenerateOptions() {
        UpgradeManager mgr = GameObject.FindWithTag("UpgradeManager").GetComponent<UpgradeManager>();
        yield return new WaitUntil(() => mgr.AvailableUpgrades.Count != 0);
        List<Upgrade> upgradePool = new List<Upgrade>(mgr.AvailableUpgrades);
        foreach (Transform child in _buttons.transform ) {
            // choose one from the upgrade pool
            Upgrade randomUpgrade = upgradePool[Random.Range(0, upgradePool.Count)];
            // remove this one from the pool
            upgradePool.Remove(randomUpgrade);
            child.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade = randomUpgrade;

        }
        yield return null;
    }


}
