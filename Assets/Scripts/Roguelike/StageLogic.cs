using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class StageLogic : MonoBehaviour {
    [SerializeField] ClusterCollection[] _clusterCollections;

    [SerializeField] UnityStringEvent cueStageBanner;
    [SerializeField] UnityIntEvent updateMulch, shroomieUpdateCost;
    [SerializeField] UnityBoolEvent _invokeCelebrate;

    [SerializeField] float _stageBeginWaitDelay;
    [SerializeField] float _interstageDelay;
    [SerializeField] int _numStagesPerWorldIncludingBoss;
    [SerializeField] GameObject _upgradeFrame, _uiCanvas, _playerDragArea, _buyShroomieButton;

    public int WorldNumber = 1;
    public int StageNumber = 1;

    public int AccumulatedMulch = 0;
    public int ShroomieBaseCost = 500;

    IEnumerator BeginRoguelikeRun() {
        WorldNumber = 1; StageNumber = 1;
        while (StageNumber < _numStagesPerWorldIncludingBoss) {
            /*
             * 1-1: difficulty of (1*2)+1 = 3
             *  - Spawns 8 clusters -> 7 to 9 clusters
             * 1-2: difficulty of (1*2)+2 = 4
             *  - Spawns 11 clusters -> 10 to 12 clusters
             * 1-3: difficulty of (1*2)+3 = 5
             *  - Boss stage
             * 
             * 2-1: difficulty of (2*2)+1 = 5
             *  - Spawns 13 clusters -> 12 to 14 clusters
             * 2-2: difficulty of (2*2)+2 = 6
             *  - Spawns 16 clusters -> 15 to 17 clusters
             * 2-3: difficulty of (2*2)+3 = 7
             *  - Boss stage
             * 
             * 3-1: difficulty of (3*2)+1 = 7
             *  - Spawns 19 clusters -> 18 to 20 clusters
             * 3-2: difficulty of (3*2)+2 = 8
             *  - Spawns 24 clusters -> 23 to 25 clusters
             * 3-3: difficulty of (3*2)+3 = 9
             */

            int difficulty = (WorldNumber * 2) + StageNumber;

            setPlayerControls(true);
            //toggleShooting(true);
            //GameObject.FindWithTag("Player").GetComponent<PlayerShooting>().ExtraBulletUpgradeLevel = StageNumber - 1; // for testing only.

            AudioManager.Instance.PlayMusic("Shroomies Next Spread");
            cueStageBanner.Invoke(WorldNumber + "-" + StageNumber);
            yield return new WaitForSeconds(_stageBeginWaitDelay);

            // allow player to buy shroomies
            _buyShroomieButton.SetActive(true);
            _buyShroomieButton.GetComponent<Animator>().Play("ShroomieButtonFadeIn");
            shroomieUpdateCost.Invoke((int)((1 + (difficulty / 10f)) * ShroomieBaseCost) - 150); // cost scales on difficulty.

            int numClustersToSpawn = (int)Mathf.Ceil(UnityEngine.Random.Range(2 * Mathf.Pow(difficulty, 1.1f), 2 * Mathf.Pow(difficulty, 1.1f) + 2));
            int currNumClustersElapsed = 0;
            while (currNumClustersElapsed < numClustersToSpawn) {
                // choose which collection to use to spawn with
                ClusterCollection chosenCollection = Array.Find(_clusterCollections, c => c.WorldNumber == WorldNumber && c.StageNumber == StageNumber);
                // randomly choose which cluster to spawn from this collection
                GameObject chosenClusterPrefabToSpawn = Instantiate(chosenCollection.Clusters[UnityEngine.Random.Range(0, chosenCollection.Clusters.Length)], GameObject.FindWithTag("EnemyContainer").transform);
                // scale cluster speed depending on difficulty
                chosenClusterPrefabToSpawn.GetComponent<ClusterSettings>().MovementSpeed *= (1 + (difficulty / 10f) - .15f);
                Debug.Log("speed set to " + chosenClusterPrefabToSpawn.GetComponent<ClusterSettings>().MovementSpeed + " by multiplying by " + (1 + (difficulty / 10f) - .15f) + " where difficulty = " + difficulty);

                foreach (Transform child in chosenClusterPrefabToSpawn.transform) {
                    // attach needed event connections:
                    // 1) Reward mulch event
                    if (child.CompareTag("Enemy")) {
                        EnemyOnHit enemyOnHit = child.GetComponent<EnemyOnHit>();
                        enemyOnHit.GiveMulch.AddListener(increaseMulch);
                        enemyOnHit.MaxHealth = (int) Mathf.Clamp((enemyOnHit.MaxHealth * (Mathf.Pow(1.01f, 1.01f * difficulty) - .4f)), 1f, Mathf.Pow(2f, 16f));
                        enemyOnHit.setCurrHealthToMaxHealth();
                    }
                }
                ClusterSettings currClusterSettings = chosenClusterPrefabToSpawn.GetComponent<ClusterSettings>();
                // spawn clusters more aggressively at higher difficulties.
                float minWait = currClusterSettings.NextClusterMinDelay / (Mathf.Pow(1.1f, 1.2f * difficulty) - .4f), maxWait = currClusterSettings.NextClusterMaxDelay / (Mathf.Pow(1.1f, 1.2f * difficulty) - .4f);
                yield return new WaitForSeconds(UnityEngine.Random.Range(minWait, maxWait));
                currNumClustersElapsed++;
            }

            //yield return new WaitForSeconds(_interstageDelay);
            // wait until all enemies are dead first
            yield return new WaitUntil(() => GameObject.FindWithTag("EnemyContainer").transform.childCount == 0);
            setPlayerControls(false);
            _buyShroomieButton.GetComponent<Animator>().Play("ShroomieButtonFadeOut");
            toggleShooting(false);
            // play shroomies celebration
            AudioManager.Instance.StopAllMusic(true);
            yield return new WaitForSeconds(.5f);
            Celebrate(true);
            yield return new WaitForSeconds(1f);
            AudioManager.Instance.PlayMusic("Where To Infect");
            if (StageNumber < _numStagesPerWorldIncludingBoss) {
                // open up normal upgrades
                GameObject upgradeFrame = Instantiate(_upgradeFrame, _uiCanvas.transform);
                upgradeFrame.transform.SetAsFirstSibling();
                yield return new WaitUntil(() => upgradeFrame.activeInHierarchy == false);
                GameObject.Destroy(upgradeFrame);
            } else {
                // open up special shop
            }

            // end shroomies celebration
            Celebrate(false);
            

            StageNumber++;

        }


        yield return null;
    }

    public void Celebrate(bool val) {
        _invokeCelebrate.Invoke(val);
        if (val) {
            AudioManager.Instance.PlaySFX("Player Clear Wave");
        }
    }

    private void Start() {
        StartCoroutine(BeginRoguelikeRun());
    }

    public void increaseMulch(int amount) {
        //Debug.Log("in here " + amount);
        AccumulatedMulch += amount;
        AudioManager.Instance.PlaySFX("Player Get Mulch");
        updateMulch.Invoke(AccumulatedMulch);
    }

    public void decreaseMulch(int amount) {
        AccumulatedMulch -= amount;
        updateMulch.Invoke(AccumulatedMulch);
    }

    public void setPlayerDrag(bool newVal) {
        _playerDragArea.SetActive(newVal);
    }

    public void setPlayerControls(bool newVal) {
        setPlayerDrag(newVal);
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().CanMove = newVal;
    }

    public void onPlayerDeath() {
        setPlayerControls(false);
        _buyShroomieButton.SetActive(false);
        Invoke("restartGame", 13f);
    }

    void restartGame() {
        SceneManager.LoadScene(0);
    }

    void toggleShooting(bool val) {
        GameObject.FindWithTag("Player").GetComponent<PlayerShooting>().Toggle = val;
        GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomiesUpgradeController>().Toggle = val;
    }


}
