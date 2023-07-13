using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] float _moveSpeed;

    Vector3 _worldPos;

    [Header("Player Bounds")] public float XMin, XMax, YMin, YMax;

    Rigidbody2D _rigidBody; // _playerPrefab's rigidbody2D component.

    private void Start() {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        _worldPos = transform.position;
        float verticalAxis = Input.GetAxisRaw("Vertical");
        float horizontalAxis = Input.GetAxisRaw("Horizontal");

        Vector3 movementVector = new Vector3(horizontalAxis, verticalAxis, 0);
        movementVector = movementVector.normalized * _moveSpeed * Time.deltaTime;
        transform.position += movementVector;
    }

    public void UpdateDragPos(Vector3 posDelta) {
        transform.position += posDelta;
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, XMin, XMax),  Mathf.Clamp(transform.position.y, YMin, YMax));
    }

}
