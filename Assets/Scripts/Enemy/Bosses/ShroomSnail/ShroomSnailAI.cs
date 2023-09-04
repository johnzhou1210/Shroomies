using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;



public class ShroomSnailAI : EnemyShooting {
    // when boss dies, kill all minions

    bool _phase2 = false, _phase3 = false;
    [SerializeField] ClusterCollection _minionCollection, _minionCollection2;
    [SerializeField] float _minMinionSpawnTime = 4f, _maxMinionSpawnTime = 8f;

    private void Start() {
        Animator = GetComponent<Animator>();
        StateManager = GetComponent<EnemyStateManager>();
        if (BarrelConfigurations.Length > 0) {
            CurrentBarrelConfiguration = BarrelConfigurations[0];
        } else {
            Debug.LogError(transform.name + " does not have any set barrel configuration!");
        }
        ExecuteAI();
    }

    public new void ExecuteAI() {
        StartCoroutine(monsterBehavior());
    }

    enum SnailBossAction {
        WIDE_SWEEP,
        NECK_ATTACK,
        TRIPLE_SHOT,
        ROLLOUT,
        NONE
    }

    public void PlayBelchSound() {
        AudioManager.Instance.PlaySFX("Snail Boss Belch");
    }

    public void PlayTailSweep() {
        AudioManager.Instance.PlaySFX("Snail Boss Tail Swoosh");
    }

    IEnumerator MinionSpawn() {
        var collection = GameObject.FindWithTag("Roguelike Manager").GetComponent<StageLogic>().StageNumber == 5 ? _minionCollection : _minionCollection2;
        while (StateManager.CurrentState != StateManager.DeadState) {
            float waitTime = UnityEngine.Random.Range(_minMinionSpawnTime, _maxMinionSpawnTime);
            yield return new WaitForSeconds(waitTime);
            GameObject chosenCluster = Instantiate(collection.Clusters[UnityEngine.Random.Range(0, collection.Clusters.Length)], GameObject.FindWithTag("EnemyContainer").transform);
            float difficulty = GameObject.FindWithTag("Roguelike Manager").GetComponent<StageLogic>().Difficulty;
            // scale cluster speed depending on difficulty
            chosenCluster.GetComponent<ClusterSettings>().MovementSpeed *= (1 + ( difficulty / 10f) - .15f);
            Debug.Log("speed set to " + chosenCluster.GetComponent<ClusterSettings>().MovementSpeed + " by multiplying by " + (1 + (difficulty / 10f) - .15f) + " where difficulty = " + difficulty);

            foreach (Transform child in chosenCluster.transform) {
                GameObject.FindWithTag("Roguelike Manager").GetComponent<StageLogic>().AddEnemyListeners(child, GameObject.FindWithTag("Roguelike Manager").GetComponent<StageLogic>().StageNumber == 5 ? difficulty / 2 : difficulty );
            }
            yield return new WaitForSeconds(waitTime);
        }
                
        yield return null;
    }

    private void Update() {
        if (GetComponent<SnailBossOnHit>().CurrentHealth <= GetComponent<SnailBossOnHit>().MaxHealth*.67f && !_phase2 && !_phase3) {
            _phase2 = true;
            AudioManager.Instance.PlaySFX("Snail Boss Phase 2");
            GetComponent<SnailBossOnHit>().StartCoroutine(GetComponent<SnailBossOnHit>().ExplosionEffect(32, 1.4f));
            FireRate = 1f;
            Animator.speed = .9f;
            // spawn minions every 4-8 seconds.
            StartCoroutine(MinionSpawn());
        } else if (GetComponent<SnailBossOnHit>().CurrentHealth <= GetComponent<SnailBossOnHit>().MaxHealth * .33f && !_phase3) {
            _phase3 = true;
            AudioManager.Instance.PlaySFX("Snail Boss Phase 2");
            GetComponent<SnailBossOnHit>().StartCoroutine(GetComponent<SnailBossOnHit>().ExplosionEffect(50, 1.4f));
            Animator.speed = 1f;
            FireRate = 0f;
        } else {
            Animator.speed = .8f;
            FireRate = 1f;
        }

    }

    IEnumerator monsterBehavior() {
        Animator.speed = Mathf.Clamp(1 / FireRate, 1f, 16f);
        /*
     * SNAIL BOSS AI
     * - CONSISTS OF 4 MOVES:
     *      - WIDE SWEEP
     *          - Play wide sweep animation
     *          - Attach sweep attack events to each attack moment
     *          - Sweeping bullets destroy both player and enemy bullets.
     *      - NECK ATTACK
     *          - Play neck attack animation
     *          - Make boss' neck harmful
     *          - Make boss' sprite invulnerable but neck vulnerable
     *      - TRIPLE SHOT
     *          - Play triple shot animation
     *          - Attach triple attack event to attack moment
     *      - ROLLOUT
     *          - Play prepare animation
     *              - Attach event to end of prepare animation to play one of the 3 types of rollouts as listed below.
     *          - Make rolling shell harmful
     *          - Make boss invulnerable
     *          - 3 Types (refer to sketch):
     *              1) Spiral
     *              2) Zig-zag
     *              3) Looping
     * - ATTACK MOVE INTERVAL DECREASES AS HEALTH DECREASES.
     * - MINIONS SPAWN WHEN BOSS IS <= 50% HP.
     */

        SnailBossAction previousAction = SnailBossAction.NONE;

        SnailBossAction chooseSnailBossAction() {
            SnailBossAction newAction = (SnailBossAction) UnityEngine.Random.Range(0, Enum.GetNames(typeof(SnailBossAction)).Length);
            while (newAction == SnailBossAction.NONE ||  newAction == previousAction) {
                newAction = (SnailBossAction)UnityEngine.Random.Range(0, Enum.GetNames(typeof(SnailBossAction)).Length);
            }
            return newAction;
        }

        
        yield return new WaitForSeconds(7f);
        while (StateManager.CurrentState != StateManager.DeadState) {
            // fire rate here is more like speed in between actions
            //Debug.Log("in loop. current state is " + StateManager.CurrentState);
            yield return new WaitForSeconds(FireRate / 2f);

            // choose a random SnailBossAction
            SnailBossAction chosenAction = chooseSnailBossAction();

            switch(chosenAction) {
                case SnailBossAction.NONE:
                    Debug.LogError("SnailBossAction cannot be NONE!");
                    break;
                case SnailBossAction.WIDE_SWEEP:
                    Animator.Play("ShroomSnailWideSweep");
                    BulletVelocity = 4f;
                    CurrentBarrelConfiguration = BarrelConfigurations[1];
                    CurrentBulletType = BulletType.SNAIL_BOSS_WIDE;
                    yield return new WaitForSeconds(1f + FireRate / 2);
                    CurrentBarrelConfiguration = BarrelConfigurations[2];
                    yield return new WaitForSeconds(1f + FireRate / 2);
                    CurrentBarrelConfiguration = BarrelConfigurations[1];
                    yield return new WaitForSeconds(2.8f + FireRate);
                    break;
                case SnailBossAction.NECK_ATTACK:
                    Animator.Play("SnailNeckAttack");
                    yield return new WaitForSeconds(.5f);
                    AudioManager.Instance.PlaySFX("Spring Sound");
                    yield return new WaitForSeconds(3.5f + FireRate);
                    break;
                case SnailBossAction.TRIPLE_SHOT:
                    Animator.Play("ShroomSnailTripleShot");
                    BulletVelocity = 5.5f;
                    CurrentBarrelConfiguration = BarrelConfigurations[0];
                    CurrentBulletType = BulletType.SNAIL_BOSS_BELCH;
                    yield return new WaitForSeconds(.25f + FireRate / 2);
                    CurrentBarrelConfiguration = BarrelConfigurations[3];
                    yield return new WaitForSeconds(.35f + FireRate / 2);
                    CurrentBarrelConfiguration = BarrelConfigurations[4];
                    yield return new WaitForSeconds(1f + FireRate);
                    break;
                case SnailBossAction.ROLLOUT:
                    Animator.Play("ShroomSnailRolloutPrepare");
                    yield return new WaitForSeconds(.75f);
                    // choose a random rollout
                    AudioManager.Instance.PlaySFX("Snail Boss Rollout Sound");
                    int chosenNum = UnityEngine.Random.Range(1, 4);
                    Animator.Play("ShroomSnailRollout" + chosenNum);
                    switch(chosenNum) {
                        case 1:
                            yield return new WaitForSeconds(9.5f + FireRate);
                            break;
                        case 2:
                            yield return new WaitForSeconds(10f + FireRate);
                            break;
                        case 3:
                            yield return new WaitForSeconds(10.5f + FireRate);
                            break;
                    }
                    break;
            }
            previousAction = chosenAction;
            yield return new WaitForSeconds(FireRate / 2f);

        }

        Debug.Log("exit boss loop");
        yield return null;
    }


}
