using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public static UpgradeController Instance;

    [SerializeField] List<GameObject> upgradePrefabs;
    private List<GameObject> availableUpgrades = new List<GameObject>();

    private List<GameObject> upgradeOptions = new List<GameObject>();

    [SerializeField] GameObject nextAdventureButton;
    [SerializeField] GameObject worldMapButton;
    [SerializeField] GameObject tryAgainButton;

    List<int> upgradesDone = new List<int>();

    private int levelUps;

    private int goldTrickle;
    private int doubleTap;
    private int extraDamage;
    private int damageBlock;
    private int blockHits;
    private int extraExp;
    private int lowerCoinCost;
    private int extraGold;
    private int spawnSpeed;

    private float goldTrickleTime = 8f;
    private float goldTrickleTimer;

    private bool victory;

    private int step = 0;

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

        if (PlayerPrefs.HasKey("BlockHits"))
        {
            blockHits = PlayerPrefs.GetInt("BlockHits");
        }
        upgradesDone.Add(blockHits);

        if (PlayerPrefs.HasKey("DamageBlock"))
        {
            damageBlock = PlayerPrefs.GetInt("DamageBlock");
        }
        upgradesDone.Add(damageBlock);

        if (PlayerPrefs.HasKey("DoubleTap"))
        {
            doubleTap = PlayerPrefs.GetInt("DoubleTap");
        }
        upgradesDone.Add(doubleTap);

        if (PlayerPrefs.HasKey("ExtraDamage"))
        {
            extraDamage = PlayerPrefs.GetInt("ExtraDamage");
        }
        upgradesDone.Add(extraDamage);

        if (PlayerPrefs.HasKey("ExtraExp"))
        {
            extraExp = PlayerPrefs.GetInt("ExtraExp");
        }
        upgradesDone.Add(extraExp);

        if (PlayerPrefs.HasKey("ExtraGold"))
        {
            extraGold = PlayerPrefs.GetInt("ExtraGold");
        }
        upgradesDone.Add(extraGold);

        if (PlayerPrefs.HasKey("GoldTrickle"))
        {
            goldTrickle = PlayerPrefs.GetInt("GoldTrickle");
        }
        upgradesDone.Add(goldTrickle);

        if (PlayerPrefs.HasKey("LowerCoinCost"))
        {
            lowerCoinCost = PlayerPrefs.GetInt("LowerCoinCost");
        }
        upgradesDone.Add(lowerCoinCost);

        if (PlayerPrefs.HasKey("SpawnSpeed"))
        {
            spawnSpeed = PlayerPrefs.GetInt("SpawnSpeed");
        }
        upgradesDone.Add(spawnSpeed);


        //TO-DO If new game:
        foreach (GameObject prefab in upgradePrefabs)
        {
            string upgradeName = prefab.name;
            int temp = prefab.GetComponent<UpgradeButton>().Amount - PlayerPrefs.GetInt(upgradeName);
            step++;
            for (int i = 0; i < temp; i++)
            {
                availableUpgrades.Add(prefab);
            }
        }
    }

    public void NewGame()
    {
        blockHits = 0;
        damageBlock = 0;
        doubleTap = 0;
        extraDamage = 0;
        extraExp = 0;
        extraGold = 0;
        goldTrickle = 0;
        lowerCoinCost = 0;
        spawnSpeed = 0;
        upgradesDone.Clear();
        availableUpgrades.Clear();

        foreach (GameObject prefab in upgradePrefabs)
        {
            string upgradeName = prefab.name;
            int temp = prefab.GetComponent<UpgradeButton>().Amount - PlayerPrefs.GetInt(upgradeName);
            step++;
            for (int i = 0; i < temp; i++)
            {
                availableUpgrades.Add(prefab);
            }
        }
    }

    private void Update()
    {
        if (goldTrickle != 0)
        {
            GoldTrickle();
        }
    }

    private void GoldTrickle()
    {
        if (GameController.Instance.GetGameType() == GameType.Game)
        {
            goldTrickleTimer += Time.deltaTime;
            if (goldTrickle > 0)
            {
                if (goldTrickleTimer >= goldTrickleTime / goldTrickle)
                {
                    goldTrickleTimer = 0;
                    GameController.Instance.AddCoin(1);
                }
            }
        }
    }

    public void SetLevelUps(int levelUps)
    {
        this.levelUps = levelUps;
    }

    public void LeveledUp()
    {
        if (levelUps > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (availableUpgrades.Count != 0)
                {
                    int random = Random.Range(0, availableUpgrades.Count);
                    if (!CheckForDuplicate(availableUpgrades[random]))
                    {
                        GameObject upgrade = Instantiate(availableUpgrades[random], this.transform);
                        availableUpgrades.Remove(upgrade);
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }
        levelUps--;
    }

    public void SetResult(bool victory)
    {
        this.victory = victory;
    }

    private bool CheckForDuplicate(GameObject upgrade)
    {
        if (upgradeOptions.Contains(upgrade))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void UpgradeSelected(string name)
    {
        switch (name)
        {
            case "goldTrickle": goldTrickle++; PlayerPrefs.SetInt("GoldTrickle", goldTrickle); break;
            case "doubleTap": doubleTap++; PlayerPrefs.SetInt("DoubleTap", doubleTap); break;
            case "extraDamage": extraDamage++; PlayerPrefs.SetInt("ExtraDamage", extraDamage); break;
            case "damageBlock": damageBlock++; PlayerPrefs.SetInt("DamageBlock", damageBlock); break;
            case "blockHits": blockHits++; PlayerPrefs.SetInt("BlockHits", blockHits); BattleController.Instance.BlockHitAdded(); break;
            case "extraExp": extraExp++; PlayerPrefs.SetInt("ExtraExp", extraExp); break;
            case "lowerCoinCost": lowerCoinCost++; PlayerPrefs.SetInt("LowerCoinCost", lowerCoinCost); break;
            case "extraGold": extraGold++; PlayerPrefs.SetInt("ExtraGold", extraGold); break;
            case "spawnSpeed": spawnSpeed++; PlayerPrefs.SetInt("SpawnSpeed", spawnSpeed); break;
        }
        int children = this.transform.childCount;

        for (int i = children - 1; i >= 0; i--)
        {
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }
        if (levelUps > 0)
        {
            LeveledUp();
        }
        else
        {
            if (victory)
            {
                nextAdventureButton.SetActive(true);
            }
            else
            {
                tryAgainButton.SetActive(true);
            }
            worldMapButton.SetActive(true);
        }
    }

    public int DoubleTap
    {
        get { return doubleTap; }
    }

    public int ExtraDamage
    {
        get { return extraDamage; }
    }

    public int DamageBlock
    {
        get { return damageBlock; }
    }

    public int BlockHits
    {
        get { return blockHits; }
    }

    public int ExtraExp
    {
        get { return extraExp; }   
    }

    public int LowerCoinCost
    {
        get { return lowerCoinCost; }
    }

    public int ExtraGold
    {
        get { return extraGold; }   
    }

    public int SpawnSpeed
    {
        get { return spawnSpeed; }
    }
}
