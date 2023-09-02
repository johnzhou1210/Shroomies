using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerStateManager))]
public class PlayerShooting : MonoBehaviour {
    public bool Toggle = false;
    float _cooldown = 1f;
    [SerializeField] float _baseFireRate = .5f, _baseCritRate = .05f, _baseBulletVelocity = 8f;
    [SerializeField] int _baseAttackPower = 3, _basePierceCount = 0;


    [Range(.01f, 5f)] public float FireRate = .5f;
    [Range(0, 8f)] public float BulletVelocity = 5f;
    [Range(0, 64)] public int AttackPower = 3;
    [Range(0, 1f)] public float CritRate = .05f;
    [Range(0, 32f)] public int PierceCount = 0, BulletClearLimit = 0;
    public bool BulletsBounce = false;
    public int ExtraBulletUpgradeLevel = 0;

    public BulletType CurrentBulletType;
    public ShroomiesUpgradeController ShroomiesController;


    Animator _animator;
    PlayerStateManager _stateManager;
    [SerializeField] List<BarrelConfiguration> _barrelConfigurations;
    BarrelConfiguration _currentBarrelConfiguration;

    private void Start() {
        ShroomiesController = GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomiesUpgradeController>();

        _animator = GetComponent<Animator>();
        _stateManager = GetComponent<PlayerStateManager>();
        _currentBarrelConfiguration = _barrelConfigurations[ExtraBulletUpgradeLevel];
        FireRate = _baseFireRate;
        PierceCount = _basePierceCount;
        BulletVelocity = _baseBulletVelocity;
        AttackPower = _baseAttackPower;
        CritRate = _baseCritRate;

        ShroomiesController.AttackPower = AttackPower / 5;

    }

    private void OnDisable() {
        StopAllCoroutines();
        // also deactivate all bullets in pool?
    }


    private void Update() {
        _cooldown = Mathf.Clamp(_cooldown - Time.deltaTime, 0, FireRate);
        if (Toggle && _cooldown <= 0f && _stateManager.CurrentState == _stateManager.AliveState) {
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
        _animator.Play("PlayerShoot");
        yield return null;
    }

    public void Fire() {
        StartCoroutine(fire());
    }

    IEnumerator fire() {
        // shoot bullets depending on current barrel configuration.
        AudioManager.Instance.PlaySFX("Player Shoot Sound");
        BulletDamageInfo dmgInfo = new BulletDamageInfo(BulletVelocity, AttackPower, CritRate, PierceCount, BulletsBounce, BulletClearLimit);
        _currentBarrelConfiguration.Fire(CurrentBulletType, BulletOwnershipType.PLAYER, dmgInfo);
        Debug.Log("Shot bullet");
        yield return null;
    }

    public void RateOfFireUpgrade(float reduction, bool shroomItUp) {
        FireRate = _baseFireRate * (1 - reduction);
        if (shroomItUp) {
            ShroomiesController.FireRate = FireRate * 4;
        }

    }

    public void FirePowerUpgrade(float increase, bool shroomItUp) {
        AttackPower = (int)Mathf.Ceil(_baseAttackPower * (1 + increase));
        if (shroomItUp) {

            ShroomiesController.AttackPower = AttackPower / 4;
        }
    }

    public void CritUpgrade(float newPercent, bool shroomItUp) {
        CritRate = _baseCritRate + newPercent;
        if (shroomItUp) {
            ShroomiesController.CritRate = CritRate;
        }
    }

    public void PierceUpgrade(Int32 newNumber, bool shroomItUp) {
        Debug.Log("recieved " + newNumber);
        PierceCount = _basePierceCount + newNumber;
        if (shroomItUp) {
            ShroomiesController.PierceCount = PierceCount;
        }
    }

    public void ExtraShotUpgrade(Int32 newNumber, bool shroomItUp) {
        Debug.Log("recieved " + newNumber);
        ExtraBulletUpgradeLevel = newNumber;
        float multFactor = 1;
        switch (newNumber) {
            case 0: multFactor = 1f; break;
            case 1: multFactor =  (1f / 1.75f); break;
            case 2: multFactor = (1f / 1.75f); break;
            case 3: multFactor = (1f / 1.75f); break;
        }
        AttackPower = (int)Mathf.Ceil(AttackPower * multFactor);
        if (shroomItUp) {
            ShroomiesController.ExtraBulletUpgradeLevel = ExtraBulletUpgradeLevel;
            Debug.Log(AttackPower + " divided by 4");
            ShroomiesController.AttackPower = AttackPower / 4;
        }
    }

    public void WideShotUpgrade(Int32 newNumber, bool shroomItUp) {
        Debug.Log("recieved " + newNumber);
        switch (newNumber) {
            case 0:
                CurrentBulletType = BulletType.NORMAL; break;
            case 1:
                CurrentBulletType = BulletType.WIDE1; break;
            case 2:
                CurrentBulletType = BulletType.WIDE2; break;
            case 3:
                CurrentBulletType = BulletType.WIDE3; break;
        }
        if (shroomItUp) {
            ShroomiesController.CurrentBulletType = CurrentBulletType;
        }
    }

    public void RicochetUpgrade(Boolean val, bool shroomItUp) {
        Debug.Log("recieved " + val);
        BulletsBounce = val;
        ShroomiesController.BulletsBounce = BulletsBounce;
    }

}
