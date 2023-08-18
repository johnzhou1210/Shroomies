using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShotBarrelWidthSpacing : MonoBehaviour
{
    Transform _leftBarrel, _rightBarrel;

    private void Start() {
        _leftBarrel = transform.Find("Left");
        _rightBarrel = transform.Find("Right");
    }

    private void OnEnable() {
        _leftBarrel = transform.Find("Left");
        _rightBarrel = transform.Find("Right");
    }

    public void setSpacing(BulletType bulletType) {
        Debug.Log("set spacing called " + bulletType);
        Debug.Log(_leftBarrel); Debug.Log(_rightBarrel);
        // determine if current shroomie is a player or a shroomie
        GameObject prefab = transform.parent.parent.gameObject;
        if (prefab.CompareTag("Shroomie")) {
            switch (bulletType) {
                case BulletType.NORMAL:
                    _leftBarrel.localPosition = new Vector3(-.1f, 0, 0);
                    _rightBarrel.localPosition = new Vector3(.1f, 0, 0);
                    break;
                case BulletType.WIDE1:
                    _leftBarrel.localPosition = new Vector3(-.1f, 0, 0);
                    _rightBarrel.localPosition = new Vector3(.1f, 0, 0);
                    break;
                case BulletType.WIDE2:
                    _leftBarrel.localPosition = new Vector3(-.15f, 0, 0);
                    _rightBarrel.localPosition = new Vector3(.15f, 0, 0);
                    break;
                case BulletType.WIDE3:
                    _leftBarrel.localPosition = new Vector3(-.22f, 0, 0);
                    _rightBarrel.localPosition = new Vector3(.22f, 0, 0);
                    break;
            }
        } else {
            switch (bulletType) {
                case BulletType.NORMAL:
                    _leftBarrel.localPosition = new Vector3(-.075f, 0, 0);
                    _rightBarrel.localPosition = new Vector3(.075f, 0, 0);
                    break;
                case BulletType.WIDE1:
                    _leftBarrel.localPosition = new Vector3(-.075f, 0, 0);
                    _rightBarrel.localPosition = new Vector3(.075f, 0, 0);
                    break;
                case BulletType.WIDE2:
                    _leftBarrel.localPosition = new Vector3(-.15f, 0, 0);
                    _rightBarrel.localPosition = new Vector3(.15f, 0, 0);
                    break;
                case BulletType.WIDE3:
                    _leftBarrel.localPosition = new Vector3(-.225f, 0, 0);
                    _rightBarrel.localPosition = new Vector3(.225f, 0, 0);
                    break;
            }
        }


        
        
    }
}
