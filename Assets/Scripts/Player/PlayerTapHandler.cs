using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Double tap original source: https://stackoverflow.com/questions/43771179/how-to-detect-single-and-double-click-in-unity */

public class PlayerTapHandler : MonoBehaviour {
    public event Action OnSingleTap;
    public event Action OnDoubleTap;
    [Tooltip("Defines the maximum time between two taps to make it double tap")]
    [SerializeField] private float tapThreshold = 0.25f;
    Action updateDelegate;
    private float tapTimer = 0.0f;
    private bool tap = false;

    public static PlayerTapHandler Instance;

    private void Awake() {
        Instance = this;
#if UNITY_EDITOR || UNITY_STANDALONE
        updateDelegate = UpdateEditor;
#elif UNITY_IOS || UNITY_ANDROID
        updateDelegate = UpdateMobile;
#endif
    }
    private void Update() {
        updateDelegate?.Invoke();
    }
    private void OnDestroy() {
        OnSingleTap = null;
        OnDoubleTap = null;
    }
#if UNITY_EDITOR || UNITY_STANDALONE
    private void UpdateEditor() {
        if (Input.GetMouseButtonDown(0)) {
            if (Time.time < this.tapTimer + this.tapThreshold) {
                OnDoubleTap?.Invoke();
                this.tap = false;
                return;
            }
            this.tap = true;
            this.tapTimer = Time.time;
        }
        if (this.tap == true && Time.time > this.tapTimer + this.tapThreshold) {
            this.tap = false;
            OnSingleTap?.Invoke();
        }
    }
#elif UNITY_IOS || UNITY_ANDROID
    private void UpdateMobile ()
    {
        for(int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if(Input.GetTouch(i).tapCount == 2)
                {
                    if(OnDoubleTap != null){ OnDoubleTap();}
                }
                if(Input.GetTouch(i).tapCount == 1)
                {
                    if(OnSingleTap != null) { OnSingleTap(); }
                }
            }
        }
    }
#endif

    private void Start() {
        //OnSingleTap = () => {
        //    Debug.Log("Single tap");
        //};
        //OnDoubleTap = () => {
        //    Debug.Log("Double tap");
        //};
    }

}
