using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour {

    private PlayerInputActions playerInputActions;
    [SerializeField] float moveSpeed = 6f;

    private Vector3 worldPos;

    public float XMin, XMax, YMin, YMax;
    public bool CanMove = true;

    [SerializeField] PlayerOnHit _plrOnHit;

    Rigidbody2D _rigidBody; // _playerPrefab's rigidbody2D component.

    private void OnDestroy() {
        DragPlayer.OnUpdatePlayerPosition -= UpdateDragPos;
    }


    private void Start() {
        DragPlayer.OnUpdatePlayerPosition += UpdateDragPos;
        _rigidBody = GetComponent<Rigidbody2D>();
        playerInputActions = InputManager.InputActions;
        
    }

    private void FixedUpdate() {

        if (!_plrOnHit.Dead && CanMove) {
            worldPos = transform.position;
            //float verticalAxis = Input.GetAxisRaw("Vertical");
            //float horizontalAxis = Input.GetAxisRaw("Horizontal");

            Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();

            Vector3 movementVector = new Vector3(inputVector.x, inputVector.y, 0);
            movementVector = movementVector.normalized * moveSpeed * Time.deltaTime;
            transform.Translate(movementVector);
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, XMin, XMax), Mathf.Clamp(transform.position.y, YMin, YMax));
        }

        
    }

    public void UpdateDragPos(Vector3 posDelta) {
        transform.Translate(posDelta);
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, XMin, XMax), Mathf.Clamp(transform.position.y, YMin, YMax));
    }

}
