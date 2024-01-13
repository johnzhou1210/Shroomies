using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class SelectedObject : MonoBehaviour
{
    Animator animator;
    static string SelectedButton;
    public bool isSelected = false;
    GameObject selectedObject;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        StartCoroutine(OnSelectedButton());
    }

    IEnumerator NewSelection() {
        while(selectedObject != EventSystem.current.currentSelectedGameObject) {

            if (EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>() != null) {
                EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>().Play("ButtonSelect");

                selectedObject = EventSystem.current.currentSelectedGameObject;

                if (gameObject.GetComponentInParent<Animator>().name != EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>().name) {
                    animator.Play("ButtonDeselect");
                }
            }
            else if(EventSystem.current.currentSelectedGameObject.GetComponent<Animator>() == null) {
                animator.Play("ButtonDeselect");
            }
            //else if (EventSystem.current.currentSelectedGameObject.GetComponent<Animator>() == null) { }
            EventBroker.CallPaletteChange();
            yield return null;
        }
        EventBroker.CallPaletteChange();
        yield return null;
    }

    IEnumerator OnSelectedButton() {
        while(EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>() != null) {
            if(gameObject.name == EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>().name) {
                animator.Play("ButtonSelect");
            }
            else if(gameObject.name != EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>().name) {
                animator.Play("ButtonDeselect");
            }
            EventBroker.CallPaletteChange();
            yield return null;
        }
        while(EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>() == null) {
            animator.Play("ButtonDeselect");
            EventBroker.CallPaletteChange();
            yield return null;
        }
        EventBroker.CallPaletteChange();
        yield return null;
    }

    private void CurrentSelection() {
        if (gameObject.name == EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>().name && 
                isSelected == false) {
            animator.Play("ButtonSelect");
            EventBroker.CallPaletteChange();
            isSelected = true;
        } else if(gameObject.name != EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>().name) {
            animator.Play("ButtonDeselect");
            EventBroker.CallPaletteChange();
            isSelected = false;
        }
    }
}
