using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerStateManager))]
public class PlayerShooting : MonoBehaviour {
    [Range(.01f, 5f)] public float FireRate = .5f;
    [Range(0, 8f)] public float BulletVelocity = 5f;
    public bool Toggle = false;
    public int AttackPower = 100;
    [Range(0, 1f)] public float CritRate = .05f;
    [Range(0, 32f)] public int PierceCount = 0, BulletClearLimit = 0;
    public bool BulletsBounce = false;
    public int ExtraBulletUpgradeLevel = 0;

    public BulletType CurrentBulletType;
    public ShroomiesUpgradeController ShroomiesController;



    [SerializeField] private float baseFireRate = .5f, baseCritRate = .05f, baseBulletVelocity = 8f;
    [SerializeField] private int baseAttackPower = 3, basePierceCount = 0;
    [SerializeField] private List<BarrelConfiguration> _barrelConfigurations;

    private float cooldown = 1f;

    private Animator animator;
    private PlayerStateManager stateManager;
    private BarrelConfiguration currentBarrelConfiguration;
    private PlayerInputActions playerInputActions;


    private void Awake() {
        playerInputActions = InputManager.InputActions;
    }

    private void Start() {
        ShroomiesController = GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomiesUpgradeController>();

        animator = GetComponent<Animator>();
        stateManager = GetComponent<PlayerStateManager>();
        currentBarrelConfiguration = _barrelConfigurations[ExtraBulletUpgradeLevel];
        FireRate = baseFireRate;
        PierceCount = basePierceCount;
        BulletVelocity = baseBulletVelocity;
        AttackPower = baseAttackPower;
        CritRate = baseCritRate;

        ShroomiesController.AttackPower = AttackPower / 5;

    }

    private void OnDisable() {
        StopAllCoroutines();
        // also deactivate all bullets in pool?
    }


    private void Update() {
        cooldown = Mathf.Clamp(cooldown - Time.deltaTime, 0, FireRate);
        if (Toggle && cooldown <= 0f && stateManager.CurrentState == stateManager.AliveState) {
            // shoot bullet
            StartCoroutine(FireAnimCoroutine());
            // reset cooldown
            cooldown = FireRate;
        }
        if (currentBarrelConfiguration != _barrelConfigurations[ExtraBulletUpgradeLevel]) {
            currentBarrelConfiguration = _barrelConfigurations[ExtraBulletUpgradeLevel];
        }

    }

    IEnumerator FireAnimCoroutine() {
        // play animation
        animator.speed = 1 / FireRate;
        animator.Play("PlayerShoot");
        yield return null;
    }

    public void Fire() {
        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine() {
        // shoot bullets depending on current barrel configuration.
        AudioManager.Instance.PlaySFX("Player Shoot Sound");
        BulletDamageInfo dmgInfo = new BulletDamageInfo(BulletVelocity, AttackPower, CritRate, PierceCount, BulletsBounce, BulletClearLimit);
        currentBarrelConfiguration.Fire(CurrentBulletType, BulletOwnershipType.PLAYER, dmgInfo);
        yield return null;
    }

    public void RateOfFireUpgrade(float reduction, bool shroomItUp) {
        FireRate = baseFireRate * (1 - reduction);
        if (shroomItUp) {
            ShroomiesController.FireRate = FireRate * 4;
        }

    }

    public void FirePowerUpgrade(float increase, bool shroomItUp) {
        AttackPower = (int)Mathf.Ceil(baseAttackPower * (1 + increase));
        if (shroomItUp) {

            ShroomiesController.AttackPower = AttackPower / 4;
        }
    }

    public void CritUpgrade(float newPercent, bool shroomItUp) {
        CritRate = baseCritRate + newPercent;
        if (shroomItUp) {
            ShroomiesController.CritRate = CritRate;
        }
    }

    public void PierceUpgrade(Int32 newNumber, bool shroomItUp) {
        PierceCount = basePierceCount + newNumber;
        if (shroomItUp) {
            ShroomiesController.PierceCount = PierceCount;
        }
    }

    public void ExtraShotUpgrade(Int32 newNumber, bool shroomItUp) {
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
            ShroomiesController.AttackPower = AttackPower / 4;
        }
    }

    public void WideShotUpgrade(Int32 newNumber, bool shroomItUp) {
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
        BulletsBounce = val;
        ShroomiesController.BulletsBounce = BulletsBounce;
    }

}
