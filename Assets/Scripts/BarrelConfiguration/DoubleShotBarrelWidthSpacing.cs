using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShotBarrelWidthSpacing : MonoBehaviour
{
    Transform _leftBarrel, _rightBarrel;

    private void Start() {
        _leftBarrel = transform.Find("Left").transform;
        _rightBarrel = transform.Find("Right").transform;
    }

    public void setSpacing(BulletType bulletType) {
        //Debug.Log("set spacing called " + bulletType);
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
                _leftBarrel.localPosition = new Vector3(-.115f, 0, 0);
                _rightBarrel.localPosition = new Vector3(.115f, 0, 0);
                break;
            case BulletType.WIDE3:
                _leftBarrel.localPosition = new Vector3(-.155f, 0, 0);
                _rightBarrel.localPosition = new Vector3(.155f, 0, 0);
                break;
        }
    }
}
