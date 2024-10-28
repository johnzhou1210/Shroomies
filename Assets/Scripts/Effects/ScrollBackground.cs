using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    public float HorizontalSpeed = 0;
    public float VerticalSpeed = .2f;
    new Renderer renderer;

    private void Start() {
        renderer = GetComponent<Renderer>();
    }

    private void FixedUpdate() {
        Vector2 offset = new Vector2(Time.fixedTime * HorizontalSpeed, Time.fixedTime * VerticalSpeed);
        renderer.material.mainTextureOffset = offset;
    }

}
