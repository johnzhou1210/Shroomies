using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting.InputSystem;

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
        if (EventSystem.current.currentSelectedGameObject != null &&
            EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>() != null) {
            if (gameObject.name == EventSystem.current.currentSelectedGameObject.GetComponentInParent<Animator>().name) {
                animator.Play("ButtonSelect");
            } else {
                animator.Play("ButtonDeselect");
            }
        } else {
            animator.Play("ButtonDeselect");
        }
        EventBroker.CallPaletteChange();
    }

}
