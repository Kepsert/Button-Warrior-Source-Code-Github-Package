using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum CurrentBattleState { Fight, ScrollForward, ScrollBackward, Ranged, Clear }
public class BattleController : MonoBehaviour
{
    public static BattleController Instance;

    [SerializeField] GameObject EnemyPanel;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI attackTimerText;
    [SerializeField] TextMeshProUGUI attackTypeText;

    private CurrentBattleState battleState = CurrentBattleState.ScrollForward;

    private int attackTime;

    private float attackIntervalTimer;
    private int attackIntervalTime;

    private int enemyHP;

    private int maxHP = 0;

    private bool fightStarted = false;

    int blockHits;

    bool rangedAttack = false;
    bool rangedDone = false;

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
    }

    private void Start()
    {
        blockHits = UpgradeController.Instance.BlockHits;
    }

    public void ClearEnemyPanel()
    {
        EnemyPanel.SetActive(false);
    }

    public void SetEnemyPanel(string name, string text, int currentHP, int maxHP, int waitTime)
    {
        EnemyPanel.SetActive(true);
        this.maxHP = maxHP;
        nameText.text = name;
        attackTypeText.text = text;
        livesText.text = currentHP + "/" + maxHP;
        attackTimerText.text = "Next attack: " + waitTime.ToString();
    }

    public void UpdateHP(int currentHP)
    {
        livesText.text = currentHP + "/" + maxHP;
    }

    public void UpdateAttackTime(int time)
    {
        attackTimerText.text = "Next attack: " + time.ToString();
    }

    public void SetFightStarted(bool fight)
    {
        fightStarted = fight;
    }

    public bool GetFightStarted()
    {
        return fightStarted;
    }

    public bool CanBlockHit()
    {
        if (blockHits > 0)
        {
            blockHits--;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BlockHitAdded()
    {
        blockHits++;
    }

    public CurrentBattleState GetBattleState()
    {
        return battleState;
    }

    public void SetBattleState (CurrentBattleState state)
    {
        battleState = state;   
    }

    private void Update()
    {

    }

    public void StartRangedAttack(string text, string name, int maxHP, int waitTime)
    {
        if (!rangedAttack)
        {
            attackIntervalTime = waitTime;
            EnemyPanel.SetActive(true);
            nameText.text = name;
            livesText.text = maxHP + "/" + maxHP;
            attackTimerText.text = "Ranged attack: " + waitTime;
            attackTypeText.text = text;
            rangedAttack = true;
        }
    }

    public void EndRangedAttack()
    {
        EnemyPanel.SetActive(false);
    }

    public void StartBattle(string name, int maxHP, int currentHP, int attackInterval)
    {
        fightStarted = true;
        attackIntervalTime = attackInterval;
        enemyHP = maxHP;
        this.maxHP = maxHP;
        EnemyPanel.SetActive(true);
        nameText.text = name;
        livesText.text = currentHP + "/" + maxHP;
        attackTimerText.text = "Next attack: " + attackInterval;
    }

    public void EndBattle()
    {
        EnemyPanel.SetActive(false);
        fightStarted = false;
        attackIntervalTime = 0;
        attackTime = 0;
        attackIntervalTimer = 0;
        GameController.Instance.KillEnemy();
    }

    public bool FightStarted()
    {
        return fightStarted;
    }


    /* Old Update
     * 
     * if (enemyHP > 0 && fightStarted)
{
    attackIntervalTimer += Time.deltaTime;
    attackTime = attackIntervalTime - Mathf.FloorToInt(attackIntervalTimer);
    attackTimerText.text = "Next attack: " + attackTime;
    if (attackIntervalTimer >= attackIntervalTime)
    {
        attackIntervalTimer = 0;
    }
}

if (rangedAttack && !rangedDone)
{
    attackIntervalTimer += Time.deltaTime;
    attackTime = attackIntervalTime - Mathf.FloorToInt(attackIntervalTimer);
    attackTimerText.text = "Ranged attack: " + attackTime;
    if (attackIntervalTimer >= attackIntervalTime)
    {
        attackIntervalTimer = 0;
        rangedAttack = false;
        rangedDone = true;
    }
}*/
}
