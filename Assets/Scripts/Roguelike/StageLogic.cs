using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class StageLogic : MonoBehaviour {
    [SerializeField] ClusterCollection[] _clusterCollections;

    [SerializeField] UnityStringEvent cueStageBanner;
    [SerializeField] UnityIntEvent updateMulch;

    [SerializeField] float _stageBeginWaitDelay;
    [SerializeField] float _interstageDelay;
    [SerializeField] int _numStagesPerWorldIncludingBoss;
    [SerializeField] GameObject _upgradeFrame, _uiCanvas, _playerDragArea;

    [SerializeField] PlayerTapHandler _tapHandler;

    public int WorldNumber = 1;
    public int StageNumber = 1;

    int accumulatedMulch = 0;

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

            setPlayerControls(true);
            GameObject.FindWithTag("Player").GetComponent<PlayerShooting>()._toggle = true;
            //GameObject.FindWithTag("Player").GetComponent<PlayerShooting>().ExtraBulletUpgradeLevel = StageNumber - 1; // for testing only.


            AudioManager.Instance.PlayMusic("Shroomies Next Spread");
            cueStageBanner.Invoke(WorldNumber + "-" + StageNumber);
            yield return new WaitForSeconds(_stageBeginWaitDelay);

            int difficulty = (WorldNumber * 2) + StageNumber;
            int numClustersToSpawn = (int)Mathf.Ceil(UnityEngine.Random.Range(2 * Mathf.Pow(difficulty, 1.1f), 2 * Mathf.Pow(difficulty, 1.1f) + 2));
            int currNumClustersElapsed = 0;

            while (currNumClustersElapsed < numClustersToSpawn) {
                // choose which collection to use to spawn with
                ClusterCollection chosenCollection = Array.Find(_clusterCollections, c => c.WorldNumber == WorldNumber && c.StageNumber == StageNumber);
                // randomly choose which cluster to spawn from this collection
                GameObject chosenClusterPrefabToSpawn = Instantiate(chosenCollection.Clusters[UnityEngine.Random.Range(0, chosenCollection.Clusters.Length)], GameObject.FindWithTag("EnemyContainer").transform);
                
                foreach (Transform child in chosenClusterPrefabToSpawn.transform) {
                    // attach needed event connections:
                    // 1) Reward mulch event
                    if (child.CompareTag("Enemy")) {
                        EnemyOnHit enemyOnHit = child.GetComponent<EnemyOnHit>();
                        enemyOnHit.giveMulch.AddListener(increaseMulch);
                    }
                }
                ClusterSettings currClusterSettings = chosenClusterPrefabToSpawn.GetComponent<ClusterSettings>();
                // spawn clusters more aggressively at higher difficulties.
                float minWait = currClusterSettings.NextClusterMinDelay / ( Mathf.Pow(1.1f, 1.2f*difficulty) - .4f), maxWait = currClusterSettings.NextClusterMaxDelay / (Mathf.Pow(1.1f, 1.2f * difficulty) - .4f);
                yield return new WaitForSeconds(UnityEngine.Random.Range(minWait, maxWait));
                currNumClustersElapsed++;
            }
            
            //yield return new WaitForSeconds(_interstageDelay);
            // wait until all enemies are dead first
            yield return new WaitUntil(() => GameObject.FindWithTag("EnemyContainer").transform.childCount == 0);
            setPlayerControls(false);
            GameObject.FindWithTag("Player").GetComponent<PlayerShooting>()._toggle = false;
            AudioManager.Instance.PlayMusic("Where To Infect");
            if (StageNumber < _numStagesPerWorldIncludingBoss) {
                // open up normal upgrades
                GameObject upgradeFrame = Instantiate(_upgradeFrame, _uiCanvas.transform);
                yield return new WaitUntil(() => upgradeFrame.activeInHierarchy == false);
                GameObject.Destroy(upgradeFrame);
            } else {
                // open up special shop
            }
            
            StageNumber++;

        }


        yield return null;
    }

    private void Start() {
        StartCoroutine(BeginRoguelikeRun());
    }

    public void increaseMulch(int amount) {
        //Debug.Log("in here " + amount);
        accumulatedMulch += amount;
        AudioManager.Instance.PlaySFX("Player Get Mulch");
        updateMulch.Invoke(accumulatedMulch); 
    }

    public void setPlayerDrag(bool newVal) {
        _playerDragArea.SetActive(newVal);
    }

    public void setPlayerControls(bool newVal) {
        setPlayerDrag(newVal);
        _tapHandler.enabled = newVal;
    }

  

}
