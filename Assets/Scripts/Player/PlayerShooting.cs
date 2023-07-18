using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[RequireComponent(typeof(PlayerStateManager))]
public class PlayerShooting : MonoBehaviour
{
    [HideInInspector] public bool _toggle = false;
    float _cooldown = 1f;

    [Range(.01f, 5f)] public float FireRate = .25f;
    [Range(0, 8f)] public float BulletVelocity = 5f;
    [Range(0, 64)] public int AttackPower = 1;
    [Range(0, 100f)] public float CritRate = 5;
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



}
