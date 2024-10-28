using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class DragPlayer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public static event Action<Vector3> OnUpdatePlayerPosition;
    
    private Vector2 lastDragPos = Vector2.zero, posDelta = Vector2.zero;
    [SerializeField] float maxDragSpeed = 1f;

    private void Start() {
        addPhysics2DRaycaster();
#if !UNITY_ANDROID && !UNITY_IOS
    gameObject.SetActive(false);
#endif
    }

    void addPhysics2DRaycaster() {
        if (GameObject.FindObjectOfType<Physics2DRaycaster>() == null) {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
        Debug.Log("Begin drag");
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        Vector2 dragPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 fingerDownPos = Camera.main.ScreenToWorldPoint(eventData.pressPosition);
        lastDragPos = lastDragPos == Vector2.zero ? fingerDownPos : lastDragPos;
        posDelta = (dragPos - lastDragPos);
        // clamp magnitude of posDelta
        posDelta = Vector2.ClampMagnitude(posDelta, maxDragSpeed);
        OnUpdatePlayerPosition?.Invoke(posDelta);
        lastDragPos = dragPos;

    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        Debug.Log("End drag");
        lastDragPos = Vector2.zero;
    }

}




