using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShroomiesUpgradeController : MonoBehaviour
{
    public bool Toggle = true;

    // set upgrades based on active shroom-it-up upgrades.
    [Range(.01f, 5f)] public float FireRate = .5f;
    [Range(0, 8f)] public float BulletVelocity = 5f;
    [Range(0, 64)] public int AttackPower = 3;
    [Range(0, 1f)] public float CritRate = .05f;
    [Range(0, 32f)] public int PierceCount = 0;

    public bool BulletsBounce = false;
    public int ExtraBulletUpgradeLevel = 0;
    public BulletType CurrentBulletType;

    public UnityEvent RequestShroomiesUpgradeUpdate;

    private void OnEnable() {
        Toggle = GameObject.FindWithTag("Player").GetComponent<PlayerShooting>().Toggle;
    }

    private void Start() {
    }

    void onSingleTap() {
    }

    void onDoubleTap() {
        Toggle = !Toggle;
    }


    public void RequestUpgrade() {
        RequestShroomiesUpgradeUpdate.Invoke();
    }

}
