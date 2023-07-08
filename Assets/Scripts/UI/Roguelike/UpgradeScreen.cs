using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeScreen : MonoBehaviour
{
    Animator _animator;
    bool _selected = false;
    GameObject SelectedButton = null;
    [SerializeField] GameObject _title, _buttons, _desc;
    [SerializeField] GameObject _contents;
    public bool Upgrading = false;



    private void OnEnable() {
        _animator = GetComponent<Animator>();
        onShow();
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
                }
            }
            // confirm selection
            // play confirm animation on buttonObj
            AudioManager.Instance.PlaySFX("Player Get Upgrade");
            buttonObj.GetComponent<Animator>().Play("UpgradeButtonConfirmed");
            GetComponent<Animator>().Play("UpgradeFrameFadeOut");
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
            setDescription("You have chosen upgrade " + buttonObj.name + ".");
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
    }



}
