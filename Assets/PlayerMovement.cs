using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] Transform _playerPrefab;
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _plrLeftBound, _plrRightBound, _plrTopBound, _plrBottomBound;

    public Vector3 WorldPos;

    Rigidbody2D _rigidBody; // _playerPrefab's rigidbody2D component.

    private void Start() {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        WorldPos = _playerPrefab.position;

        float verticalAxis = Input.GetAxisRaw("Vertical");
        float horizontalAxis = Input.GetAxisRaw("Horizontal");

        Vector3 movementVector = new Vector3(horizontalAxis, verticalAxis, 0);
        movementVector = movementVector.normalized * _moveSpeed * Time.deltaTime;
        _playerPrefab.position += movementVector;
        // regulate position
        _playerPrefab.position = new Vector3(
            Mathf.Clamp(_playerPrefab.position.x, _plrLeftBound, _plrRightBound),
            Mathf.Clamp(_playerPrefab.position.y, _plrBottomBound, _plrTopBound),
            0
            );

        //Debug.Log(transform.position);
    }
}
