using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    public float horizontalSpeed = 0;
    public float verticalSpeed = .2f;
    Renderer _renderer;

    private void Start() {
        _renderer = GetComponent<Renderer>();
    }

    private void FixedUpdate() {
        Vector2 offset = new Vector2(Time.fixedTime * horizontalSpeed, Time.fixedTime * verticalSpeed);
        _renderer.material.mainTextureOffset = offset;
    }

}
