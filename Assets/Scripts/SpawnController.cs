using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public static SpawnController Instance;

    [SerializeField] List<GameObject> enemyPrefabs;

    [SerializeField] int amountOfEnemies;

    [Range(6, 25)]
    [SerializeField] int spawnInterval = 8;
    float spawnTimer;

    [SerializeField] int currentLevel;
    [SerializeField] string nextLevel;

    bool enemyOnScreen = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GameController.Instance.SetGameType(GameType.Game);
        GameController.Instance.SetAmountOfEnemies(amountOfEnemies);
        GameController.Instance.SetScrollType(ScrollBackground.Scroll);
        GameController.Instance.Init();
        RhythmController.Instance.Init();

        GameController.Instance.SetCurrentLevel(currentLevel);
        GameController.Instance.SetNextLevel(nextLevel);
        
        FightShopController.Instance.Init();
    }

    public void SetEnemyOnScreen(bool check)
    {
        enemyOnScreen = check;
    }

    private void Update()
    {
        if (GameController.Instance.GetScrollType() == ScrollBackground.Scroll && !enemyOnScreen)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= (spawnInterval+UpgradeController.Instance.SpawnSpeed) && amountOfEnemies > 0)
            {
                Debug.Log("spawning enemy");
                enemyOnScreen = true;
                spawnTimer = 0;
                int random = Random.Range(0, enemyPrefabs.Count);
                Instantiate(enemyPrefabs[random], this.transform.position, Quaternion.identity);
                amountOfEnemies--;
            }
        }
    }
}
