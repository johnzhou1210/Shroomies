using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Drawing;
using Unity.VisualScripting;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.EventSystems;

public class UpgradeScreen : MonoBehaviour
{
    Animator _animator;
    bool _selected = false;
    GameObject SelectedButton = null;
    [SerializeField] GameObject _title, _buttons, _desc, _upgradeName, _upgradeBack, _shroomItUp;
    [SerializeField] GameObject _contents, selectedObject;
    public bool Upgrading = false;
    public Button primaryButton;

    public UnityUpgradeBoolEvent onPlayerUpgrade;


    private void OnEnable() {
        _animator = GetComponent<Animator>();
        primaryButton.Select();
        onShow();
    }

    private void Start() {
        onPlayerUpgrade.AddListener(GameObject.FindWithTag("UpgradeManager").GetComponent<UpgradeManager>().OnUpgrade);
    }

    private void Update() {
            StartCoroutine(NewSelection());
            Debug.Log(selectedObject.name);
    }

    public void ShowShroomiesDescription(bool val) {
        if (val) {
            setDescription(SelectedButton.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade.ShroomItUpDescription, 
                SelectedButton.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade.shroomItUpKeyWords);
        } else {
            setDescription(SelectedButton.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade.UpgradeDescription, 
                SelectedButton.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade.descriptionKeyWords);
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
                GameObject.FindWithTag("Roguelike Manager").GetComponent<StageLogic>().incrementShroomItUps();
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

    void setDescription(string description, List<string> descriptionKeyWords)
    {

        string[] newWord = description.Split(' ');

        string newDescription = "";

       for(int i = 0; i < newWord.Length; i++)
        {
            foreach (string keyword in descriptionKeyWords)
            {
                if (newWord[i] == keyword)
                {
                    newWord[i] = ("<color=#" + ChangePalette.holder.color2.ToHexString() + ">" + keyword + "</color>");
                }
            }

            if (i == newWord.Length - 1)
            {
                newDescription += newWord[i];
            }
            else{
                newDescription += newWord[i] + " ";
            }

            _desc.GetComponent<TextMeshProUGUI>().text = newDescription;
        }
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
        InputManager.ToggleActionMap(InputManager.inputActions.Player);
    }

    public void onShow() {
        Debug.Log("onshow called");
        setDescription("Tap on an upgrade to view its description.");
        setTitle("Choose an upgrade!");
        setShroomItUpVisibility(false);
        
        GetComponent<Animator>().Play("UpgradeFrameFadeIn");

        InputManager.ToggleActionMap(InputManager.inputActions.UI);
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

    IEnumerator NewSelection() {
        while (selectedObject != EventSystem.current.currentSelectedGameObject) {
            selectedObject = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.currentSelectedGameObject.GetComponent<UIPaletteChange>().PaletteColor = 2;
            foreach (Transform child in _buttons.transform ) {
                if(child.name != EventSystem.current.currentSelectedGameObject.name){
                    Debug.Log(child.name + EventSystem.current.currentSelectedGameObject.name);
                    child.GetComponent<UIPaletteChange>().PaletteColor = 1;
                }
            }
            EventBroker.CallPaletteChange();
            yield return null;
        }
        EventBroker.CallPaletteChange();
        yield return null;
    }


}
