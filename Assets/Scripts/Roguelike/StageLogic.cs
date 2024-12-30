using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class StageLogic : MonoBehaviour {
    [SerializeField] ClusterCollection[] _clusterCollections;

    /* EVENTS */
    
    public static event Action<EntityDisplayInfo> OnCueBossBanner;
    public static event Action<string> OnCueStageBanner;
    public static event Action<int> OnUpdateMulch, OnShroomieUpdateCost;
    public static event Action<bool> OnInvokeCelebrate, OnInvokeGameOver;
    
    
    [SerializeField] UnityEntityDisplayInfoEvent _cueBossBanner;
    [SerializeField] UnityIntEvent updateMulch, shroomieUpdateCost;
    [SerializeField] UnityBoolEvent _invokeCelebrate, _invokeGameOver;

    [SerializeField] float _stageBeginWaitDelay;
    [SerializeField] float _interstageDelay;
    [SerializeField] int _numStagesPerWorldIncludingBoss, _bossStage = 7, _bossStage2 = 13;
    [SerializeField] GameObject _upgradeFrame, _uiCanvas, _playerDragArea, _buyShroomieButton, _gameOverEffect, _resultsScreen, _thankYouScreen, _pauseMenu;

    public GameSettings GameSettings;
    public ChangePalette ChangePalette;

    public UnityBoolEvent InvokeEnableBossHPDisplay;

    [HideInInspector] public int WorldNumber = 1;
    [HideInInspector] public int StageNumber = 1;

    [SerializeField] int StartWorld = 1;
    [SerializeField] int StartStage = 1;

    public int AccumulatedMulch = 0;
    public int AccumulatedShroomies = 0;
    public int AccumulatedShroomItUps = 0;
    public int CurrentShroomItUpCost = 0;
    public int ShroomieBaseCost = 25;

    public int previousShroomiesCost = 25;

    public float Difficulty = 0f;

    bool _gameOver = false;
    bool accelerateSplat = false;
    int _clearTime = 0, _enemiesKilled = 0, _earnedMulch = 0;

    IEnumerator CountClearTime() {
        while (!_gameOver) {
            yield return new WaitForSeconds(1f);
            _clearTime++;
        }
        yield return null;

    }

    IEnumerator BeginRoguelikeRun() {
        StartCoroutine(CountClearTime());
        WorldNumber = StartWorld; StageNumber = StartStage;
        while (StageNumber <= _numStagesPerWorldIncludingBoss) {
            setPlayerControls(false);
            Difficulty = (WorldNumber * 2) + StageNumber;

            if (StageNumber != _bossStage && StageNumber !=_bossStage2) { AudioManager.Instance.PlayMistaDJ(); }//{ AudioManager.Instance.PlayMusic("s Next Spread"); }
            OnCueStageBanner?.Invoke(StageNumber == _bossStage || StageNumber == _bossStage2 ? "<color=#" + ChangePalette.Holder.color2.ToHexString() + ">" + WorldNumber + "-" + StageNumber + "</color>" : WorldNumber + "-" + StageNumber);
            
            yield return new WaitForSeconds(_stageBeginWaitDelay);

            // choose which collection to use to spawn with
            ClusterCollection chosenCollection = Array.Find(_clusterCollections, c => c.WorldNumber == WorldNumber && c.StageNumber == StageNumber);

            // If not boss stage, spawn clusters immediately. Otherwise, spawn boss.
            if (StageNumber != _bossStage && StageNumber != _bossStage2) {
                setPlayerControls(true);
                yield return new WaitForSeconds(1f);
                // allow player to buy shroomies
                loadShroomieButton(Difficulty);

                int numClustersToSpawn = 1;
                int currNumClustersElapsed = 0;
                while (currNumClustersElapsed < numClustersToSpawn && !_gameOver) {

                    // randomly choose which cluster to spawn from this collection
                    GameObject chosenClusterPrefabToSpawn = Instantiate(chosenCollection.Clusters[UnityEngine.Random.Range(0, chosenCollection.Clusters.Length)], GameObject.FindWithTag("EnemyContainer").transform);
                    // scale cluster speed depending on difficulty
                    //chosenClusterPrefabToSpawn.GetComponent<ClusterSettings>().MovementSpeed *= (1 + (Difficulty / 10f) - .15f);
                    //Debug.Log("speed set to " + chosenClusterPrefabToSpawn.GetComponent<ClusterSettings>().MovementSpeed + " by multiplying by " + (1 + (Difficulty / 10f) - .15f) + " where difficulty = " + Difficulty);

                    foreach (Transform child in chosenClusterPrefabToSpawn.transform) {
                        AddEnemyListeners(child, Difficulty);
                    }
                    ClusterSettings currClusterSettings = chosenClusterPrefabToSpawn.GetComponent<ClusterSettings>();   
                    yield return new WaitForSeconds(UnityEngine.Random.Range(currClusterSettings.NextClusterMinDelay, currClusterSettings.NextClusterMaxDelay / (Mathf.Pow(1.1f, 1.2f * Difficulty) - .4f)));
                    currNumClustersElapsed++;
                }
            } else {
                AudioManager.Instance.StopAllMusic(true);
                // This is a boss stage. Spawn the boss!
                GameObject bossClusterPrefab = Instantiate(chosenCollection.Clusters[0], GameObject.FindWithTag("EnemyContainer").transform);
                AddEnemyListeners(bossClusterPrefab.transform.GetChild(0), Difficulty);
                yield return new WaitForSeconds(3f);
                // cue boss banner
                OnCueBossBanner?.Invoke(bossClusterPrefab.transform.GetChild(0).GetComponent<DisplayData>().DisplayInfo);
                
                AudioManager.Instance.PlayMusic("It's Getting Harder");
                yield return new WaitForSeconds(4f);
                setPlayerControls(true);
                // display boss health bar
                InvokeEnableBossHPDisplay.Invoke(true);
                yield return new WaitForSeconds(1f);
                // allow player to buy shroomies
                loadShroomieButton(Difficulty);

            }

            // wait until all enemies are dead first
            yield return new WaitUntil(() => GameObject.FindWithTag("EnemyContainer").transform.childCount == 0);
            if (_gameOver) { // make sure it's not over because the player died
                yield break;
            }
            if (StageNumber == _bossStage || StageNumber == _bossStage2) {
                InvokeEnableBossHPDisplay.Invoke(false);
                AudioManager.Instance.PlaySFX("Boss Defeat Sound");
                yield return new WaitForSeconds(2f);  
            }
            setPlayerControls(false);
            _buyShroomieButton.GetComponent<Animator>().Play("ShroomieButtonFadeOut");
            toggleShooting(false);
            // play shroomies celebration
            AudioManager.Instance.StopAllMusic(true);
            yield return new WaitForSeconds(.5f);
            Celebrate(true);
            yield return new WaitForSeconds(1f);
            if (StageNumber == _numStagesPerWorldIncludingBoss) {
                // go to thank you screen
                _thankYouScreen.SetActive(true);
                StartCoroutine(ThankYouScreen());
                yield break;
            } else if (StageNumber < _numStagesPerWorldIncludingBoss) {
                // open up normal upgrades
                AudioManager.Instance.PlayMusic("Where To Infect");
                GameObject upgradeFrame = Instantiate(_upgradeFrame, _uiCanvas.transform);
                upgradeFrame.transform.SetAsFirstSibling();
                yield return new WaitUntil(() => upgradeFrame.activeInHierarchy == false);
                GameObject.Destroy(upgradeFrame);
                if (_pauseMenu.GetComponent<PauseGame>().randomPalette) {
                    ChangePalette.ChangeColorRandom();
                }
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

    public void AddEnemyListeners(Transform trans, float difficulty) {
        // attach needed event connections for when enemies spawn in:
        // 1) Reward mulch event
        if (trans.CompareTag("Enemy")) {
            EnemyOnHit enemyOnHit = trans.GetComponent<EnemyOnHit>();
            enemyOnHit.GiveMulch.AddListener(OnEnemyKill);
            //enemyOnHit.MaxHealth = (int)Mathf.Clamp((enemyOnHit.MaxHealth * (Mathf.Pow(1.03f, 1.06f * difficulty) - .4f)), 1f, Mathf.Pow(2f, 16f));
            enemyOnHit.setCurrHealthToMaxHealth();
        }
    }

    private void Start() {
        StartCoroutine(BeginRoguelikeRun());
    }

    void loadShroomieButton(float difficulty) {
        _buyShroomieButton.SetActive(true);
        _buyShroomieButton.GetComponent<Animator>().Play("ShroomieButtonFadeIn");
        //shroomieUpdateCost.Invoke((int)(StageNumber * 100)); // cost scales on difficulty.
        if (AccumulatedShroomies <= 0)
        {
            shroomieUpdateCost.Invoke((int)(Math.Max(ShroomieBaseCost, (AccumulatedShroomies * 100) + (AccumulatedShroomies + 1) * 50)));
        }
    }

    public void OnEnemyKill(int amount) {
        AccumulatedMulch += amount;
        _enemiesKilled++; _earnedMulch += amount;
        AudioManager.Instance.PlaySFX("Player Get Mulch");
        updateMulch.Invoke(AccumulatedMulch);
    }

    public void decreaseMulch(int amount) {
        AccumulatedMulch -= amount;
        updateMulch.Invoke(AccumulatedMulch);
    }

    public void incrementAccumulatedShroomies()
    {
        //shroomieUpdateCost.Invoke((int)(previousShroomiesCost + (100 + (50 * Math.Max(0, AccumulatedShroomies - 1)))));
        //previousShroomiesCost = (int)(previousShroomiesCost + (100 + (50 * Math.Max(0, AccumulatedShroomies - 1))));
        //shroomieUpdateCost.Invoke((int)(Math.Max(100, (AccumulatedShroomies * 100) + (AccumulatedShroomies + 1) * 50)));
        AccumulatedShroomies++;
    }

    public void incrementShroomItUps()
    {
        AccumulatedShroomItUps++;
    }

    public void setPlayerDrag(bool newVal) {
        _playerDragArea.SetActive(newVal);
    }

    public void setPlayerControls(bool newVal) {
        setPlayerDrag(newVal);
        _pauseMenu.SetActive(newVal);
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().CanMove = newVal;
        GameObject.FindWithTag("Player").GetComponent<PlayerOnHit>().Debounce = !newVal;
    }

    public void onPlayerDeath() {
        InputManager.ToggleActionMap(InputManager.InputActions.UI);
        GameObject.FindWithTag("Score").SetActive(false);
        _buyShroomieButton.SetActive(false);
        InvokeEnableBossHPDisplay.Invoke(false);
        AudioManager.Instance.PlaySFX("Shing");
        _gameOver = true;
        StopCoroutine(BeginRoguelikeRun());
        foreach (Transform child in GameObject.FindWithTag("EnemyContainer").transform) {
            GameObject.Destroy(child.gameObject);
        }
        _gameOverEffect.SetActive(true);
        Transform pulseEffect = _gameOverEffect.transform.Find("PulseEffect");
        _gameOverEffect.transform.Find("PulseEffect").position = new Vector3(pulseEffect.position.x, GameObject.FindWithTag("Player").transform.position.y, pulseEffect.position.z);
        setPlayerControls(false);
        Transform player = GameObject.FindWithTag("Player").transform;

        IEnumerator MovePlayerYUntilDesired(float increment, float stepDelay) {
            while (Mathf.Abs(player.position.y) > -2f) {
                player.Translate(new Vector3(0f, player.position.y < 0 ? increment * 3 : -increment, 0));
                yield return new WaitForSeconds(stepDelay);
            }
            yield return null;
        }
        IEnumerator SplatPlayer(float increment, float stepDelay, IEnumerator yCor) {
            bool skipped = false;
            float lastTime = Time.time;
            bool skipCondition() {
                if (userClickedOrTappedScreen()) {
                    skipped = true;
                }
                return Mathf.Abs(player.position.y) <= .15f || skipped;
            }
            bool skip2Condition() {
                if (userClickedOrTappedScreen()) {
                    skipped = true;
                }
                return skipped || Time.time - lastTime > 5f;
            }
            yield return new WaitUntil(skipCondition);
            yield return new WaitUntil(skip2Condition);
            lastTime = Time.time; // update time counter for finish splat skip
            // splat the player!
            while (player.position.y > -3.8f) {
                player.Translate(new Vector3(0, -increment, 0f));
                yield return new WaitForSeconds(stepDelay);
            }
            AudioManager.Instance.PlaySFX("Player Splat Sound");
            player.GetComponent<Animator>().Play("PlayerDeathSplat");
            _invokeGameOver.Invoke(true);
            yield return new WaitForSeconds(.7f);
            StopCoroutine(yCor);
            yield return new WaitUntil(() => userClickedOrTappedScreen() || Time.time - lastTime > 5f);
            StartCoroutine(ResultsScreen(false));
        }

        IEnumerator Fall() {
            yield return new WaitForSeconds(1f);
            AudioManager.Instance.PlayMusic("Player Death Sound");
            // move player to desired area of screen.
            //IEnumerator xCor = MovePlayerXUntilDesired(.01f, .01f);
            IEnumerator yCor = MovePlayerYUntilDesired(.0075f, .01f); //.0075f, 0.01f ori.
            StartCoroutine(yCor); 
            StartCoroutine(SplatPlayer(0.2f, .01f, yCor));
            yield return null;
        }
        StartCoroutine(Fall());

        
        
    }

    IEnumerator ResultsScreen(bool won) {
        bool displayCoroutineFinished = false;
        AudioManager.Instance.PlayMusic("Rising");
        _invokeGameOver.Invoke(false);
        AudioManager.Instance.PlaySFX("Cinematic Boom");
        _resultsScreen.SetActive(true);
        Transform timeElapsedText = _resultsScreen.transform.Find("TimeElapsed").transform.Find("Number"),
            enemiesKilledText = _resultsScreen.transform.Find("KillCount").transform.Find("Number"),
            mulchEarnedText = _resultsScreen.transform.Find("MulchEarned").transform.Find("Number"),
            upgradesGrid = _resultsScreen.transform.Find("UpgradesGrid").transform,
            restartText = _resultsScreen.transform.Find("Restart"),
            timeHeader = _resultsScreen.transform.Find("TimeElapsed");
        timeHeader.GetComponent<TextMeshProUGUI>().text = won ? "CLEAR TIME" : "TIME ELAPSED";

        IEnumerator StatsDisplayCoroutine() {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < _clearTime; i += (i + (_clearTime / 64) > _clearTime ? 1 : Mathf.Clamp(i / 64, 1, (i / 64) + 1))) {
                timeElapsedText.GetComponent<TextMeshProUGUI>().text = getTime(i);
                AudioManager.Instance.PlaySFX("Tick");
                yield return new WaitForSeconds(.01f);
            }
            yield return new WaitForSeconds(1f);
            if (_enemiesKilled > 0) {
                for (int i = 0; i <= _enemiesKilled; i += (i + (_enemiesKilled / 64) > _enemiesKilled ? 1 : Mathf.Clamp(i / 64, 1, (i / 64) + 1))) {
                    enemiesKilledText.GetComponent<TextMeshProUGUI>().text = i.ToString();
                    AudioManager.Instance.PlaySFX("Tick");
                    yield return new WaitForSeconds(.01f);
                }
            }
            yield return new WaitForSeconds(1f);
            if (_earnedMulch > 0) {
                for (int i = 0; i <= _earnedMulch; i += (i + (_earnedMulch / 64) > _earnedMulch ? 1 : Mathf.Clamp(i / 64, 1, (i / 64) + 1))) {
                    mulchEarnedText.GetComponent<TextMeshProUGUI>().text = i.ToString();
                    AudioManager.Instance.PlaySFX("Tick");
                    yield return new WaitForSeconds(0f);
                }
            }
            yield return new WaitForSeconds(1f);
            UpgradeManager upgMgr = GameObject.FindWithTag("UpgradeManager").GetComponent<UpgradeManager>();
            for (int i = 0; i < upgMgr.ActiveUpgrades.Count; i++) {
                Transform currEntry = upgradesGrid.Find("Entry (" + i.ToString() + ")");
                currEntry.GetComponent<Image>().color = Color.white;
                currEntry.GetComponent<Image>().sprite = upgMgr.ActiveUpgrades[i].Image;
                AudioManager.Instance.PlaySFX("Cinematic Hit");
                yield return new WaitForSeconds(.5f);
            }
            displayCoroutineFinished = true;
        }

        Coroutine statsDisplayCoroutine = StartCoroutine(StatsDisplayCoroutine());

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => userClickedOrTappedScreen()|| displayCoroutineFinished);
        // stop display coroutine regardless if it is finishe or not
        StopCoroutine(statsDisplayCoroutine);
       
        if (!displayCoroutineFinished) {
            // display the right stats in case coroutine has not finished.
            timeElapsedText.GetComponent<TextMeshProUGUI>().text = getTime(_clearTime);
            enemiesKilledText.GetComponent<TextMeshProUGUI>().text = _enemiesKilled.ToString();
            mulchEarnedText.GetComponent<TextMeshProUGUI>().text = _earnedMulch.ToString();
            UpgradeManager upgMgr = GameObject.FindWithTag("UpgradeManager").GetComponent<UpgradeManager>();
            for (int i = 0; i < upgMgr.ActiveUpgrades.Count; i++) {
                Transform currEntry = upgradesGrid.Find("Entry (" + i.ToString() + ")");
                currEntry.GetComponent<Image>().color = Color.white;
                currEntry.GetComponent<Image>().sprite = upgMgr.ActiveUpgrades[i].Image;

            }
            if (upgMgr.ActiveUpgrades.Count > 0) {
                AudioManager.Instance.PlaySFX("Cinematic Hit");
            }
            displayCoroutineFinished = true;
        }

        yield return new WaitForSeconds(1f);
        restartText.GetComponent<TextMeshProUGUI>().text = won ? "TAP TO RETURN" : "TAP TO RESTART";
        restartText.GetComponent<ResultScreenContinue>().enabled = true;
        restartText.GetComponent<ResultScreenContinue>().Won = won;
        yield return null;
    }

    IEnumerator ThankYouScreen() {
        GameObject.FindWithTag("Score").SetActive(false);
        AudioManager.Instance.PlayMusic("House Fever");
        InputManager.ToggleActionMap(InputManager.InputActions.UI);
        _thankYouScreen.SetActive(true);
        yield return new WaitForSeconds(5f);
        _thankYouScreen.transform.Find("Prompt").GetComponent<TextMeshProUGUI>().text = "PRESS ANYWHERE TO CONTINUE";
        yield return new WaitUntil(()=> InputManager.InputActions.UI.Enter.WasPressedThisFrame() || Input.touchCount > 0);
        _thankYouScreen.transform.Find("Prompt").GetComponent<TextMeshProUGUI>().text = "";
        StartCoroutine(ResultsScreen(true));
        yield return null;
    }


    void restartGame() {
        SceneManager.LoadScene(0);
    }

    void toggleShooting(bool val) {
        GameObject.FindWithTag("Player").GetComponent<PlayerShooting>().Toggle = val;
        GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomiesUpgradeController>().Toggle = val;
    }

    string getTime(int seconds) {
        int h = seconds / 3600;
        int s = seconds % 3600;
        int m = seconds / 60;
        s = seconds % 60;
        return normalizeToTwoDigits(h.ToString()) + ":" + normalizeToTwoDigits(m.ToString()) + ":" + normalizeToTwoDigits(s.ToString());
    }

    string normalizeToTwoDigits(string s) {
        if (s.Length == 1) {
            return "0" + s;
        }
        return s;
    }

    bool userClickedOrTappedScreen() {
        return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || InputManager.InputActions.UI.Enter.WasPressedThisFrame();
    }
    
}
