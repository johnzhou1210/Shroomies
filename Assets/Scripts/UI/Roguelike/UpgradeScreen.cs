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
    [SerializeField] GameObject _title, _buttons, _desc;
    [SerializeField] GameObject _contents;
    public bool Upgrading = false;

    public UnityUpgradeEvent onPlayerUpgrade;


    private void OnEnable() {
        _animator = GetComponent<Animator>();
        onShow();
    }

    private void Start() {
        onPlayerUpgrade.AddListener(GameObject.FindWithTag("UpgradeManager").GetComponent<UpgradeManager>().OnUpgrade); 
    }


    public void onClick(GameObject buttonObj) {
        if  (_selected && SelectedButton == buttonObj) {
            // hide description and title
            setDescription("");
            setTitle("");
            foreach (Transform child in _buttons.transform) {
                child.gameObject.GetComponent<Button>().enabled = false;
                if (child.gameObject != buttonObj) {
                    child.gameObject.GetComponent<Image>().enabled = false;
                    child.gameObject.GetComponent<Animator>().Play("UpgradeButtonInvisible");
                    child.gameObject.transform.Find("Image").GetComponent<Image>().enabled = false;
                }
            }
            // confirm selection
            // play confirm animation on buttonObj
            AudioManager.Instance.PlaySFX("Player Get Upgrade");
            buttonObj.GetComponent<Animator>().Play("UpgradeButtonConfirmed");
            GetComponent<Animator>().Play("UpgradeFrameFadeOut");
            // fire player upgrade event
            onPlayerUpgrade.Invoke(buttonObj.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade);

            SelectedButton = null;
        } else {
            if (_selected && SelectedButton != buttonObj) {
                // deselect previous selected button
                SelectedButton.GetComponent<Animator>().Play("UpgradeButtonUnselected");
            }
            // choose selection
            SelectedButton = buttonObj;
            _selected = true;
            setTitle("Tap again to confirm.");
            setDescription( buttonObj.transform.Find("Image").GetComponent<RenderUpgradeButton>().Upgrade.UpgradeDescription);
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
        setDescription("");
        setTitle("Choose an upgrade!");
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
