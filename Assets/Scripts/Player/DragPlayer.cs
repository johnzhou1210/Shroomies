using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable] public class Vector3Event : UnityEvent<Vector3> { }

//public class DragPlayer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
//    public Vector3Event updatePlayerPosition;
//    Vector2 _lastDragPos = Vector2.zero, _posDelta = Vector2.zero;

//    private void Start() {
//        addPhysics2DRaycaster();
//    }

//    void addPhysics2DRaycaster() {
//        if (GameObject.FindObjectOfType<Physics2DRaycaster>() == null) {
//            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
//        }
//    }

//    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
//        Debug.Log("Begin drag");
//    }

//    void IDragHandler.OnDrag(PointerEventData eventData) {
//        //Debug.Log("Dragging " + eventData.position);
//        Vector2 dragPos = Camera.main.ScreenToWorldPoint(eventData.position);
//        Vector2 fingerDownPos = Camera.main.ScreenToWorldPoint(eventData.pressPosition);
//        _lastDragPos = _lastDragPos == Vector2.zero ? fingerDownPos : _lastDragPos;
//        _posDelta = (dragPos - _lastDragPos);
//        //Debug.Log(posDelta);
//        updatePlayerPosition.Invoke(_posDelta);
//        _lastDragPos = dragPos;

//    }

//    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
//        Debug.Log("End drag");
//        _lastDragPos = Vector2.zero;
//    }

//}

public class DragPlayer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public Vector3Event updatePlayerPosition;
    [SerializeField] float _dragSpeed = 1f, _deadzoneRadius;
    [SerializeField] GameObject _startPoint, _endPoint, _player;
    //Vector2 _lastDragPos = Vector2.zero;
    GameObject _start, _end;
    public float XMin, XMax, YMin, YMax;
    Vector2 _moveVector, _fingerGoalPos, _fingerDownPos;

    private void Start() {
        addPhysics2DRaycaster();
        _start = Instantiate(_startPoint, Vector3.zero, Quaternion.identity);
        _end = Instantiate(_endPoint, Vector3.zero, Quaternion.identity);
        _moveVector = Vector2.zero;
        XMin = _player.GetComponent<PlayerMovement>().XMin;
        XMax = _player.GetComponent<PlayerMovement>().XMax;
        YMin = _player.GetComponent<PlayerMovement>().YMin;
        YMax = _player.GetComponent<PlayerMovement>().YMax;
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
        _fingerGoalPos = Camera.main.ScreenToWorldPoint(eventData.position);
        _fingerDownPos = Camera.main.ScreenToWorldPoint(eventData.pressPosition);
        Vector2 playerPos = new Vector2(_player.transform.position.x, _player.transform.position.y);


        _end.transform.position = _fingerGoalPos;
        _start.transform.position = playerPos;

        // move towards goal at a constant speed
        Vector2 dirVector = (_fingerGoalPos - playerPos);
        _moveVector = dirVector.normalized * _dragSpeed;
        Debug.Log(_moveVector);

    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        Debug.Log("End drag");
        _moveVector = Vector2.zero;
       
    }

    private void FixedUpdate() {
        if ((_fingerGoalPos - new Vector2(_player.transform.position.x, _player.transform.position.y)).magnitude  >= _deadzoneRadius) {
            _player.transform.position += new Vector3(_moveVector.x, _moveVector.y, 0);
        }
        _player.transform.position = new Vector2(Mathf.Clamp(_player.transform.position.x, XMin, XMax), Mathf.Clamp(_player.transform.position.y, YMin, YMax));

    }


}
