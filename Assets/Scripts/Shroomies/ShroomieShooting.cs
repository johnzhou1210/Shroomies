using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomieShooting : MonoBehaviour {

    [SerializeField] ShroomiesUpgradeController _controller;

    private float fireRate = .5f;
    private float bulletVelocity = 5f;
    private int attackPower = 3;
    private float critRate = .05f;
    private int pierceCount = 0, bulletClearLimit = 0;
    private bool bulletsBounce = false;
    private int extraBulletUpgradeLevel = 0;

    BulletType CurrentBulletType;


    Animator _animator;
   
    [SerializeField] List<BarrelConfiguration> _barrelConfigurations;
    BarrelConfiguration _currentBarrelConfiguration;

    private void Start() {
        
        _animator = GetComponent<Animator>();
        OnUpgradeUpdate();
        _currentBarrelConfiguration = _barrelConfigurations[extraBulletUpgradeLevel];
    }

    public void OnUpgradeUpdate() {
        ShroomiesUpgradeController source = GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomiesUpgradeController>();
        if (source != null) {
            Debug.Log("UPDATING SHROOMIE UPGRADES");
            fireRate = source.FireRate;
            pierceCount = source.PierceCount;
            bulletVelocity = source.BulletVelocity;
            attackPower = source.AttackPower;
            critRate = source.CritRate;
            extraBulletUpgradeLevel = source.ExtraBulletUpgradeLevel;
            bulletsBounce = source.BulletsBounce;
            bulletClearLimit = source.BulletClearLimit;

            switch(source.CurrentBulletType) {
                case BulletType.NORMAL: CurrentBulletType = BulletType.NORMAL_S; break;
                case BulletType.WIDE1: CurrentBulletType = BulletType.WIDE1_S; break;
                case BulletType.WIDE2: CurrentBulletType = BulletType.WIDE2_S; break;
                case BulletType.WIDE3: CurrentBulletType = BulletType.WIDE3_S; break;
            }
            
        }
    }

    private void OnDisable() {
        StopAllCoroutines();
        // also deactivate all bullets in pool?
    }

    

    private void Update() {
        if (_currentBarrelConfiguration != _barrelConfigurations[extraBulletUpgradeLevel]) {
            _currentBarrelConfiguration = _barrelConfigurations[extraBulletUpgradeLevel];
        }

    }

    public IEnumerator FireAnim() {
        // play animation
        _animator.speed = 1 / fireRate;
        _animator.Play("ShroomieShoot");
        yield return null;
    }

    public void Fire() {
        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine() {
        // shoot bullets depending on current barrel configuration.
        BulletDamageInfo dmgInfo = new BulletDamageInfo(bulletVelocity, attackPower, critRate, pierceCount, bulletsBounce, bulletClearLimit);
        _currentBarrelConfiguration.Fire(CurrentBulletType, BulletOwnershipType.PLAYER, dmgInfo);
        Debug.Log("Shot bullet");
        yield return null;
    }

}
