using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public enum ScrollBackground { Scroll, Static, Slow }
public enum GameType { MainMenu, Pause, Game, Map, Victory, Roguelite, Dead }

public enum Feedback { On, Off }
public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private GameType gameType = GameType.Game;
    [SerializeField] private ScrollBackground scrollType = ScrollBackground.Scroll;

    [SerializeField] private Feedback feedback = Feedback.On;

    private int currentNumber = 1;
    [SerializeField] GameObject scoreButtonPrefab;

    [SerializeField] GameObject[] ButtonPrefabs;

    public RectTransform panel;
    [SerializeField] GameObject scorePanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject levelSelectPanel;

    private float gameTime;
    [SerializeField] private float spawnInterval = 8f;
    private float currentSpawnInterval;
    private float spawnTimer = 0f;

    [SerializeField] private int level = 0;

    [SerializeField] TextMeshProUGUI playerLevel;
    private int characterLevel = 1;
    [SerializeField] TextMeshProUGUI enemiesLeftText;
    [SerializeField] TextMeshProUGUI livesText;

    [SerializeField] TextMeshProUGUI coinText;
    private int coins = 0;

    [SerializeField] int maxLives = 5;
    int currentLives;

    private int amountOfEnemies;

    private int lastLevelBeaten = 0;
    private int currentLevel = 0;
    private string nextLevel;

    private int levelUps;
    private int expGained;
    [SerializeField] TextMeshProUGUI expLevelText;
    [SerializeField] GameObject victoryPanel;
    [SerializeField] TextMeshProUGUI adventureResultText;
    [SerializeField] GameObject noUpgradesText;
    [SerializeField] GameObject nextAdventureButton;
    [SerializeField] GameObject worldMapButton;
    [SerializeField] GameObject tryAgainButton;
    [SerializeField] GameObject enemyPanelImage;
    [SerializeField] GameObject bottomLeftPanel;

    [SerializeField] Animator fadeAnim;

    [SerializeField] TextMeshProUGUI worldStageText;

    [SerializeField] float deathAnimationTime;
    float deathAnimationTimer;
    bool dead;

    [SerializeField] GameObject enemyAttackWarningText;

    [SerializeField] GameObject pulseNote1;
    [SerializeField] GameObject pulseNote2;
    [SerializeField] Sprite pulseNoteGreen;
    [SerializeField] Sprite pulseNoteRed;

    private int feedbackCount = 0;

    [SerializeField] GameObject mainMenuButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        if (PlayerPrefs.HasKey("LastLevelBeaten"))
        {
            lastLevelBeaten = PlayerPrefs.GetInt("LastLevelBeaten");
        }
        else
        {
            lastLevelBeaten = 0;
        }

        if (PlayerPrefs.HasKey("CharacterLevel"))
        {
            characterLevel = PlayerPrefs.GetInt("CharacterLevel");
        }
        else
        {
            characterLevel = 1;
            PlayerPrefs.SetInt("CharacterLevel", characterLevel);
        }
    }

    private void Start()
    {
        Init();

        worldStageText.text = "";
    }

    public void Init()
    {
        coins = UpgradeController.Instance.ExtraGold*25;
        coinText.text = coins.ToString();
        
        currentSpawnInterval = spawnInterval;
        currentNumber = 1;
        gameTime = 0;
        level = 0;
        expGained = 0;
        levelUps = 0;

        if (PlayerPrefs.HasKey("LastLevelBeaten"))
        {
            lastLevelBeaten = PlayerPrefs.GetInt("LastLevelBeaten");
        }
        else
        {
            lastLevelBeaten = 0;
        }

        if (PlayerPrefs.HasKey("CharacterLevel"))
        {
            characterLevel = PlayerPrefs.GetInt("CharacterLevel");
        }
        else
        {
            characterLevel = 1;
            PlayerPrefs.SetInt("CharacterLevel", characterLevel);
        }
    }

    public void ChangeMaxHealth()
    {
        characterLevel = PlayerPrefs.GetInt("CharacterLevel");
        maxLives = CharacterController.Instance.GetHealth(characterLevel);
        currentLives = maxLives;
        playerLevel.text = "Warrior Level: " + characterLevel;
        livesText.text = currentLives + "/" + maxLives;
    }

    private void Update()
    {
        if (gameType == GameType.Game)
        {
            gameTime += Time.deltaTime;
        }

        level = Mathf.FloorToInt(gameTime/10);

        if (gameType == GameType.Game) {
            currentSpawnInterval = GetSpawnInterval();

            spawnTimer += Time.deltaTime;
            if (level > 0 && spawnTimer >= currentSpawnInterval)
            {
                int randomButton;
                randomButton = Random.Range(0, ButtonPrefabs.Length);

                spawnTimer = 0f;
                GameObject button = Instantiate(ButtonPrefabs[randomButton], GetSpawnArea(ButtonPrefabs[randomButton]), Quaternion.identity, panel);
            }
        }

        if (dead)
        {
            deathAnimationTime += Time.deltaTime;
            if (deathAnimationTime >= deathAnimationTimer)
            {
                RoundEnded(false);
                dead = false;
                deathAnimationTime = 0;
            }
        }
    }

    public void SpawnButton(GameObject buttonPrefab, int coinCost)
    {
        Instantiate(buttonPrefab, GetSpawnArea(buttonPrefab), Quaternion.identity, panel);
        SpendCoins(coinCost);
    }

    public void SpawnRandomButton()
    {
        int randomButton;
        randomButton = Random.Range(0, ButtonPrefabs.Length);

        GameObject button = Instantiate(ButtonPrefabs[randomButton], GetSpawnArea(ButtonPrefabs[randomButton]), Quaternion.identity, panel);
        Debug.Log(button);
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    public void SetCurrentLives(int life)
    {
        CharacterController.Instance.PlayAnim("Warrior_TakeHit");
        life -= UpgradeController.Instance.DamageBlock * 2;
        if (life < 0)
        {
            life = 0;
        }
        currentLives -= life;

        if (currentLives <= 0) 
        {
            MusicPlayer.Instance.PlaySoundEffectByName("PlayerDead");
            currentLives = 0;
            dead = true;
            CharacterController.Instance.PlayAnim("Warrior_Death");
            SetScrollType(ScrollBackground.Static);
        }
        else
        {
            MusicPlayer.Instance.PlaySoundEffectByName("PlayerHurt");
        }
        
        livesText.text = currentLives + "/" + maxLives;
    }

    private float GetSpawnInterval()
    {
        currentSpawnInterval = spawnInterval - (float)(level / 20f);
        return currentSpawnInterval;
    }

    public void SpawnNewScoreButton()
    {
        Vector3 spawnPosition = GetSpawnArea(scoreButtonPrefab);
        GameObject scoreButton = Instantiate(scoreButtonPrefab, spawnPosition, Quaternion.identity, panel);
    }

    private Vector3 GetSpawnArea(GameObject ButtonPrefab)
    {
        /*Vector3 temp = GetBottomLeftCorner(panel) - new Vector3(Random.Range(0 - (ButtonPrefab.GetComponent<RectTransform>().rect.height * 8)
            , (panel.rect.x * 2.25f) + (ButtonPrefab.GetComponent<RectTransform>().rect.width * 8)),
            Random.Range(0 - (ButtonPrefab.GetComponent<RectTransform>().rect.height * 8)
            , (panel.rect.y * 2.25f) + (ButtonPrefab.GetComponent<RectTransform>().rect.height * 8)),
            0);
        Debug.Log("Temp: " + temp);
        Debug.Log("Transform.position: " + panel.transform.position);*/

        float minX = panel.transform.position.x + ((ButtonPrefab.GetComponent<RectTransform>().rect.width * 10) * CanvasEmpty.Instance.GetComponent<Canvas>().scaleFactor);
        float maxX = (panel.transform.position.x + (panel.rect.width * CanvasEmpty.Instance.GetComponent<Canvas>().scaleFactor)) - ((ButtonPrefab.GetComponent<RectTransform>().rect.width * 10) * CanvasEmpty.Instance.GetComponent<Canvas>().scaleFactor);
        float minY = panel.transform.position.y + ((ButtonPrefab.GetComponent<RectTransform>().rect.height * 12) * CanvasEmpty.Instance.GetComponent<Canvas>().scaleFactor);
        float maxY = (panel.transform.position.y + (panel.rect.height * CanvasEmpty.Instance.GetComponent<Canvas>().scaleFactor)) - ((ButtonPrefab.GetComponent<RectTransform>().rect.height * 12) * CanvasEmpty.Instance.GetComponent<Canvas>().scaleFactor);
        Vector3 temp = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
        return temp;
    }

    Vector3 GetBottomLeftCorner(RectTransform rt)
    {
        Vector3[] v = new Vector3[4];
        rt.GetWorldCorners(v);
        return v[0];
    }

    public GameType GetGameType()
    {
        return gameType;
    }

    public void SetGameType(GameType type)
    {
        gameType = type;

        if (gameType == GameType.Pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public ScrollBackground GetScrollType()
    {
        return scrollType;
    }

    public void SetScrollType(ScrollBackground type)
    {
        scrollType = type;
    }

    public int GetCurrentNumber()
    {
        return currentNumber;
    }

    public int GetCurrentCoins()
    {
        return coins;
    }

    public void AddToCurrentNumber()
    {
        currentNumber++;
    }

    public int GetCurrentLevel()
    {
        return level;
    }

    public void SetLevelText()
    {
        if (!PlayerPrefs.HasKey("CharacterLevel"))
        {
            playerLevel.text = "Warrior Level: " + 1;
        }
        else
        {
            playerLevel.text = "Warrior Level: " + PlayerPrefs.GetInt("CharacterLevel");
        }
    }

    public void IncreaseMaxHealth(int lives)
    {
        this.currentLives += lives;
        maxLives += lives;
        livesText.text = currentLives + "/" + maxLives;
    }

    public void LeveledUp()
    {
        playerLevel.text = "Warrior Level: " + PlayerPrefs.GetInt("CharacterLevel");
        levelUps++;
    }

    public void AddToExp(int exp)
    {
        expGained += exp;
    }

    public void AddToScore(int score)
    {
        //playerLevel.text = "Score: " + this.score;
    }

    public void AddCoin(int coins)
    {
        this.coins+=coins;
        coinText.text = this.coins.ToString();
        FightShopController.Instance.RefreshShop();
    }

    private void SpendCoins(int coins)
    {
        this.coins -= coins;
        FightShopController.Instance.RefreshShop();
        coinText.text = this.coins.ToString();
    }

    public void ChangeHP(int amount)
    {
        

        if (CharacterController.Instance.GetCharacterState() == CharacterState.Walk)
        {
            CharacterController.Instance.PlayAnim("Warrior_TakeHit");
        }
        amount -= UpgradeController.Instance.DamageBlock;
        if (amount < 0)
        {
            amount = 0;
        }
        currentLives += (amount-UpgradeController.Instance.DamageBlock);

        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }

        livesText.text = currentLives + "/" + maxLives;
    }

    public void SetAmountOfEnemies(int amount)
    {
        amountOfEnemies = amount;
        enemiesLeftText.text = amountOfEnemies.ToString();
    }

    public void KillEnemy()
    {
        amountOfEnemies--;
        if (amountOfEnemies == 0)
        {
            GameController.Instance.RoundEnded(true);
            GameController.Instance.SetGameType(GameType.Victory);
        }
        enemiesLeftText.text = amountOfEnemies.ToString();
    }

    public void RoundEnded(bool victory)
    {
        //TO DO: REPLACE WITH FADE
        MusicPlayer.Instance.FadeMusic();
        bottomLeftPanel.SetActive(false);
        UpgradeController.Instance.SetResult(victory);
        UpgradeController.Instance.SetLevelUps(levelUps);
        if (victory)
        {
            adventureResultText.text = "VICTORY";
            MusicPlayer.Instance.FadeMusic();
            MusicPlayer.Instance.ChangeBgMusic("Victory", false);
            if (currentLevel > lastLevelBeaten || lastLevelBeaten == 0)
            {
                Debug.Log("Do I get here?");
                lastLevelBeaten = currentLevel;
                LevelController.Instance.UpdateLevelSelect();
                PlayerPrefs.SetInt("LastLevelBeaten", lastLevelBeaten);
            }
        }
        else
        {
            CharacterController.Instance.SetCharacterState(CharacterState.Dead);
            MusicPlayer.Instance.FadeMusic();
            MusicPlayer.Instance.ChangeBgMusic("Lost", false);
            adventureResultText.text = "DEFEATED";
        }
        victoryPanel.GetComponent<Animator>().SetBool("Open", true);
        if (levelUps == 0)
        {
            if (victory)
            {
                nextAdventureButton.SetActive(true);
                tryAgainButton.SetActive(false);
            }
            else
            {
                nextAdventureButton.SetActive(false);
                tryAgainButton.SetActive(true);
            }
            worldMapButton.SetActive(true);
            noUpgradesText.SetActive(true);
            expLevelText.text = "You have gained " + expGained + " exp during this adventure.";
        }
        else if (levelUps > 1)
        {
            SetObjectsToFalseUponUpgradesSelection();
            UpgradeController.Instance.LeveledUp();
            expLevelText.text = "You have gained " + expGained + " exp during this adventure. This has granted you " + levelUps + " levels.";
        }
        else if (levelUps == 1)
        {
            SetObjectsToFalseUponUpgradesSelection();
            UpgradeController.Instance.LeveledUp();
            expLevelText.text = "You have gained " + expGained + " exp during this adventure. This has granted you " + levelUps + " level.";
        }

        levelUps = 0;

        SetGameType(GameType.Victory);
        SetScrollType(ScrollBackground.Static);
    }

    private void SetObjectsToFalseUponUpgradesSelection()
    {
        nextAdventureButton.SetActive(false);
        worldMapButton.SetActive(false);
        tryAgainButton.SetActive(false);
        noUpgradesText.SetActive(false);
    }

    public void SetCurrentLevel(int level)
    {
        int children = panel.transform.childCount;
        dead = false;

        for (int i = children - 1; i >= 0; i--)
        {
            if (!panel.transform.GetChild(i).name.StartsWith("Score"))
            {
                DestroyImmediate(panel.transform.GetChild(i).gameObject);
            }
        }
        currentLevel = level;
    }

    public void SetNextLevel(string level)
    {
        nextLevel = level;
    }

    public void CloseVictoryPanel()
    {
        MusicPlayer.Instance.FadeMusic();
        victoryPanel.GetComponent<Animator>().SetBool("Open", false);
    }

    public int GetLastLevelBeaten()
    {
        return lastLevelBeaten;
    }

    public void LoadScene(string name)
    {
        if (name == "TryAgain")
        {
            FightShopController.Instance.RefreshShop();
            name = SceneManager.GetActiveScene().name;
            victoryPanel.GetComponent<Animator>().SetBool("Open", false);
        }
        if (name == "Next")
        {
            FightShopController.Instance.RefreshShop();
            name = nextLevel;
        }

        fadeAnim.SetBool("Fade", true);
        StartCoroutine(LoadSceneFade(name));
    }

    private IEnumerator LoadSceneFade(string name)
    {
        float waitTime = 3.5f;
        if (!PlayerPrefs.HasKey("Narrative2"))
        {
            if (name == "MELLOW MEADOWS 5")
            {
                MusicPlayer.Instance.FadeMusic();
                name = "Narrative2";
                PlayerPrefs.SetInt("Narrative2", 1);
            }
        }

        if (name == "DUSTY DESERTS 1")
        {
            MusicPlayer.Instance.FadeMusic();
            MusicPlayer.Instance.ChangeBgMusic("LevelSelectLoop", false);
            name = "Narrative3";
        }

        if (name == "LevelSelectScene" || name.StartsWith("Narrative"))
        {
            worldStageText.text = "";
        }
        else
        {
            worldStageText.text = name;
        }
        if (name.StartsWith("MELLOW") || name.StartsWith("DUSTY") || name.StartsWith("FRIGID") || name.StartsWith("MOLTEN"))
        {
            MusicPlayer.Instance.FadeMusic();
        }
        

        yield return new WaitForSeconds(waitTime);
        if (name == "LevelSelectScene")
        {
            mainMenuButton.SetActive(true);
            MusicPlayer.Instance.ChangeBgMusic("LevelSelectLoop", true);
            panel.gameObject.SetActive(false);
            scorePanel.SetActive(false);
            shopPanel.SetActive(false);
            levelSelectPanel.SetActive(true);
            enemyPanelImage.SetActive(false);
            bottomLeftPanel.SetActive(false);
        }
        //else if (!name.StartsWith("Narrative"))
        else
        {
            mainMenuButton.SetActive(false);
            panel.gameObject.SetActive(true);
            scorePanel.SetActive(true);
            shopPanel.SetActive(true);
            levelSelectPanel.SetActive(false);
            enemyPanelImage.SetActive(true);
            bottomLeftPanel.SetActive(true);
        }


        if (!name.StartsWith("Narrative"))
        {
            fadeAnim.SetBool("Fade", false);
        }
        SceneManager.LoadScene(name);

        if (name=="LevelSelectScene")
        {
            LevelController.Instance.UpdateLevelSelect();
        }
    }

    public void SetGameTypeString(string type)
    {
        switch (type)
        {
            //public enum GameType { MainMenu, Pause, Game, Map, Victory, Roguelite }
            case "MainMenu": SetGameType(GameType.MainMenu); Time.timeScale = 1; break;
            case "Pause": SetGameType(GameType.Pause); Time.timeScale = 0; Conductor.Instance.PauseMusic(); break;
            case "Game": if (gameType == GameType.Pause) { Conductor.Instance.ResumeMusic(); } SetGameType(GameType.Game); Time.timeScale = 1; break;
            case "Map": SetGameType(GameType.Map); Time.timeScale = 1; break;
            case "Victory": SetGameType(GameType.Victory); Time.timeScale = 1; break;
        }
    }

    public void ActivateEnemyAttackWarning(int timer)
    {
        enemyAttackWarningText.SetActive(true);
        enemyAttackWarningText.GetComponent<TextMeshProUGUI>().text = "ENEMY ATTACK IN: " + timer;

        if (timer != 1 && timer != 2 && timer != 3)
        {
            enemyAttackWarningText.SetActive(false);
        }
    }

    public void PulseNotes()
    {
        var Sequence = DOTween.Sequence();
        Sequence.AppendInterval(0.03f);
        Sequence.AppendCallback(Pulse);
        Sequence.AppendInterval(0.05f);
        Sequence.AppendCallback(Shrink);
    }

    public void HitBeat(bool beat)
    {
        if (beat)
        {
            pulseNote1.GetComponent<Image>().sprite = pulseNoteGreen;
            pulseNote2.GetComponent<Image>().sprite = pulseNoteGreen;
        }
        else
        {
            pulseNote1.GetComponent<Image>().sprite = pulseNoteRed;
            pulseNote2.GetComponent<Image>().sprite = pulseNoteRed;
        }
    }

    private void Pulse()
    {
        pulseNote1.transform.DOScale(new Vector3(pulseNote1.transform.localScale.x * 1.35f, pulseNote1.transform.localScale.y * 1.2f), 0.05f);
        pulseNote2.transform.DOScale(new Vector3(pulseNote1.transform.localScale.x * 1.35f, pulseNote1.transform.localScale.y * 1.2f), 0.05f);
    }

    private void Shrink()
    {
        pulseNote1.transform.DOScale(new Vector3(1, 1), 0.1f);
        pulseNote2.transform.DOScale(new Vector3(1, 1), 0.1f);
    }

    public void TurnFeedBackOff()
    {
        feedback = Feedback.Off;
    }

    public Feedback GetFeedback()
    {
        return feedback;
    }

    public void SetFeedback(Feedback feedback)
    {
        this.feedback = feedback;
    }

    public void AddFeedback()
    {
        feedbackCount++;
        if (feedbackCount >= 5)
        {
            this.feedback = Feedback.Off;
        }
    }
}
