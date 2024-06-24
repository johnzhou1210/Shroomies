using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class DragPlayer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public UnityVector3Event updatePlayerPosition;
    Vector2 _lastDragPos = Vector2.zero, _posDelta = Vector2.zero;
    [SerializeField] float _maxDragSpeed = 1f;

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
        //Debug.Log("Begin drag");
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        //Debug.Log("Dragging " + eventData.position);
        Vector2 dragPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 fingerDownPos = Camera.main.ScreenToWorldPoint(eventData.pressPosition);
        _lastDragPos = _lastDragPos == Vector2.zero ? fingerDownPos : _lastDragPos;
        _posDelta = (dragPos - _lastDragPos);
        // clamp magnitude of posDelta
        _posDelta = Vector2.ClampMagnitude(_posDelta, _maxDragSpeed);
        updatePlayerPosition.Invoke(_posDelta);
        _lastDragPos = dragPos;

    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        //Debug.Log("End drag");
        _lastDragPos = Vector2.zero;
    }

}




