using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] DynamicJoystick _joystick;


    public Vector3 WorldPos;

    Rigidbody2D _rigidBody; // _playerPrefab's rigidbody2D component.

    private void Start() {
        _rigidBody = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate() {

        float verticalAxis = _joystick.Vertical;
        float horizontalAxis = _joystick.Horizontal;

        Vector3 movementVector = new Vector3(horizontalAxis, verticalAxis, 0);
        movementVector = movementVector.normalized * _moveSpeed * Time.deltaTime;
        transform.position += movementVector;
    }
}
