using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable] public class Vector3Event : UnityEvent<Vector3> { }

public class DragPlayer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public Vector3Event updatePlayerPosition;

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
        Debug.Log("Dragging " + eventData.position);
        Vector3 plrPos = Camera.main.ScreenToWorldPoint(eventData.position);
        plrPos.z = 0;
        updatePlayerPosition.Invoke(plrPos);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        Debug.Log("End drag");
    }

}

