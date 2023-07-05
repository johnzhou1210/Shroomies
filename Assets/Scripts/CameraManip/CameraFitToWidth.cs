using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class CameraFitToWidth : MonoBehaviour
{
    public float SceneWidth = 10f; // world distance between left and right edges of screen
    Camera _camera;

    private void Start() {
        _camera = GetComponent<Camera>();
    }

    // Adjust camera's height such that scene width fits in view even if screen/window size changes dynamically.
    private void Update() {
       fitToWidth();
    }

    void fitToWidth() {
        float unitsPerPixel = SceneWidth / Screen.width;
        float desiredHalfHeight = .5f * unitsPerPixel * Screen.height;
        _camera.orthographicSize = desiredHalfHeight;
    }

}
