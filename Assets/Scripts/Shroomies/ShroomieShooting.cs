using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomieShooting : MonoBehaviour {
    
    float _cooldown = 1f;

    float FireRate = .5f;
    float BulletVelocity = 5f;
    int AttackPower = 3;
    float CritRate = .05f;
    int PierceCount = 0;
    bool BulletsBounce = false;
    int ExtraBulletUpgradeLevel = 0;

    BulletType CurrentBulletType;


    Animator _animator;
   
    [SerializeField] List<BarrelConfiguration> _barrelConfigurations;
    BarrelConfiguration _currentBarrelConfiguration;

    private void Start() {
        
        _animator = GetComponent<Animator>();
        OnUpgradeUpdate();
        _currentBarrelConfiguration = _barrelConfigurations[ExtraBulletUpgradeLevel];
    }

    public void OnUpgradeUpdate() {
        ShroomiesUpgradeController source = GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomiesUpgradeController>();
        if (source != null) {
            Debug.Log("UPDATING SHROOMIE UPGRADES");
            FireRate = source.FireRate;
            PierceCount = source.PierceCount;
            BulletVelocity = source.BulletVelocity;
            AttackPower = source.AttackPower;
            CritRate = source.CritRate;
            ExtraBulletUpgradeLevel = source.ExtraBulletUpgradeLevel;
            BulletsBounce = source.BulletsBounce;

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
        _cooldown = Mathf.Clamp(_cooldown - Time.deltaTime, 0, FireRate);
        if (transform.parent.GetComponent<ShroomiesUpgradeController>().Toggle && _cooldown <= 0f) {
            // shoot bullet
            StartCoroutine(fireAnim());
            // reset cooldown
            _cooldown = FireRate;
        }
        if (_currentBarrelConfiguration != _barrelConfigurations[ExtraBulletUpgradeLevel]) {
            _currentBarrelConfiguration = _barrelConfigurations[ExtraBulletUpgradeLevel];
        }

    }

    IEnumerator fireAnim() {
        // play animation
        _animator.speed = 1 / FireRate;
        _animator.Play("ShroomieShoot");
        yield return null;
    }

    public void Fire() {
        StartCoroutine(fire());
    }

    IEnumerator fire() {
        // shoot bullets depending on current barrel configuration.
        BulletDamageInfo dmgInfo = new BulletDamageInfo(BulletVelocity, AttackPower, CritRate, PierceCount, BulletsBounce);
        _currentBarrelConfiguration.Fire(CurrentBulletType, BulletOwnershipType.PLAYER, dmgInfo);
        Debug.Log("Shot bullet");
        yield return null;
    }

}
