using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable] public class Vector3Event : UnityEvent<Vector3> { }

public class DragPlayer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public Vector3Event updatePlayerPosition;
    Vector2 _lastDragPos = Vector2.zero;

    private void Start() {
        addPhysics2DRaycaster();
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
        //Debug.Log("Dragging " + eventData.position);
        Vector2 dragPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 fingerDownPos = Camera.main.ScreenToWorldPoint(eventData.pressPosition);
        _lastDragPos = _lastDragPos == Vector2.zero ? fingerDownPos : _lastDragPos;
        Vector2 posDelta = (dragPos - _lastDragPos);
        //Debug.Log(posDelta);
        updatePlayerPosition.Invoke(posDelta);
        _lastDragPos = dragPos;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        Debug.Log("End drag");
        _lastDragPos = Vector2.zero;
    }

}

