using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[RequireComponent(typeof(PlayerStateManager))]
public class PlayerShooting : MonoBehaviour
{
    [HideInInspector] public bool _toggle = false;
    float _cooldown = 1f;
    [SerializeField] float _baseFireRate = .5f, _baseCritRate = .05f;
    [SerializeField] int _baseAttackPower = 3, _basePierceCount = 0;


    [Range(.01f, 5f)] public float FireRate = .5f;
    [Range(0, 8f)] public float BulletVelocity = 5f;
    [Range(0, 64)] public int AttackPower = 3;
    [Range(0, 1f)] public float CritRate = .05f;
    [Range(0, 32f)] public int PierceCount = 0;
    public bool BulletsBounce = false;
    public int ExtraBulletUpgradeLevel = 0;

    [SerializeField] BulletType _currentBulletType;


    Animator _animator;
    PlayerStateManager _stateManager;
    [SerializeField] List<BarrelConfiguration> _barrelConfigurations;
    BarrelConfiguration _currentBarrelConfiguration;

    private void Start() {
        PlayerTapHandler.Instance.OnSingleTap += onSingleTap;
        PlayerTapHandler.Instance.OnDoubleTap += onDoubleTap;
        _animator = GetComponent<Animator>();
        _stateManager = GetComponent<PlayerStateManager>();
        _currentBarrelConfiguration = _barrelConfigurations[ExtraBulletUpgradeLevel];
    }

    private void OnDisable() {
        StopAllCoroutines();
        // also deactivate all bullets in pool?
    }

    void onSingleTap() {

    }

    void onDoubleTap() {
        //Debug.Log("In here");
        _toggle = !_toggle;
    }

    private void Update() {
        _cooldown = Mathf.Clamp(_cooldown - Time.deltaTime, 0, FireRate);
        if (_toggle && _cooldown <= 0f && _stateManager.CurrentState == _stateManager.AliveState) {
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
        BulletDamageInfo dmgInfo = new BulletDamageInfo(BulletVelocity, AttackPower, CritRate, PierceCount, BulletsBounce);
        _currentBarrelConfiguration.Fire(_currentBulletType, BulletOwnershipType.PLAYER, dmgInfo);
        Debug.Log("Shot bullet");
        yield return null;
    }

    public void RateOfFireUpgrade(float reduction) {
        FireRate = _baseFireRate * (1 - reduction);
    }

    public void FirePowerUpgrade(float increase) {
        AttackPower = (int)Mathf.Ceil(_baseAttackPower * (1 + increase));
    }

    public void CritUpgrade(float newPercent) {
        CritRate = _baseCritRate + newPercent;
    }

    public void PierceUpgrade(Int32 newNumber) {
        Debug.Log("recieved " + newNumber);
        PierceCount = _basePierceCount + newNumber;
    }

    public void ExtraShotUpgrade(Int32 newNumber) {
        Debug.Log("recieved " + newNumber);
        ExtraBulletUpgradeLevel = newNumber;
    }

    public void WideShotUpgrade(Int32 newNumber) {
        Debug.Log("recieved " + newNumber);
        switch (newNumber) {
            case 0:
                _currentBulletType = BulletType.NORMAL; break;
            case 1:
                _currentBulletType = BulletType.WIDE1; break;
            case 2:
                _currentBulletType = BulletType.WIDE2; break;
            case 3:
                _currentBulletType = BulletType.WIDE3; break;
        }
    }

    public void RicochetUpgrade(Boolean val) {
        Debug.Log("recieved " + val);
        BulletsBounce = val;
    }

}
