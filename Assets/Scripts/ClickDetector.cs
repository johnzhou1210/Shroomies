using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerExitHandler,
    IPointerEnterHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("Mouse down: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("Mouse exit");
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
        Debug.Log("Begin drag");
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        Debug.Log("Dragging");
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        Debug.Log("End drag");
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
        Debug.Log("Mouse enter");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
        Debug.Log("Mouse up");
    }

    


}
