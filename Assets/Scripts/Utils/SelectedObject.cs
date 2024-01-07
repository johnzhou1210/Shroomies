using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

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
        StartCoroutine(NewSelection());
        Debug.Log(selectedObject.GetComponentInParent<Animator>().name);
    }

    IEnumerator NewSelection() {
        while(selectedObject != EventSystem.current.currentSelectedGameObject) {
            EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>().Play("ButtonSelect");
            selectedObject = EventSystem.current.currentSelectedGameObject;
            if (gameObject.GetComponentInParent<Animator>().name != EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>().name) {
                animator.Play("ButtonDeselect");
                Debug.Log(gameObject.GetComponentInParent<Animator>().name + EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>().name);
            }
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
