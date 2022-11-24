using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class Enemy : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] string name;
    [SerializeField] int exp;
    [SerializeField] int maxHP;
    int currentHP;
    [SerializeField] int level;
    [SerializeField] Animator anim;

    [Header("Melee")]
    [SerializeField] int damage;
    [SerializeField] int attackCooldown;
    [SerializeField] string attackType = "Block: Shield";

    [Header("Ranged")]
    [SerializeField] bool canRangedAttack;
    [SerializeField] int rangedDamage;
    [SerializeField] int rangedAttackCooldown;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] string getRangedAttackType;

    [Header("Animations")]
    [SerializeField] float deathAnimTime;
    float deathAnimTimer;
    [SerializeField] float attackAnimTime;
    [SerializeField] float rangedAttackAnimTime;
    [SerializeField] float specialAnimTime;

    bool tookHit = false;

    public string GetName()
    {
        return name;
    }

    public int GetExp()
    {
        return exp;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }

    public void SetCurrentHP(int HP)
    {
        currentHP = HP;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetDamage()
    {
        return damage;
    }
    
    public int GetAttackCooldown()
    {
        return attackCooldown;
    }

    public string GetAttackType()
    {
        return attackType;
    }

    public int GetRangedDamage()
    {
        return rangedDamage;
    }

    public int GetRangedAttackCooldown()
    {
        return rangedAttackCooldown;
    }

    public GameObject GetProjectilePrefab()
    {
        return projectilePrefab;
    }

    public Transform GetProjectileSpawn()
    {
        return projectileSpawn;
    }

    public string GetRangedAttackType()
    {
        return getRangedAttackType;
    }

    public float GetDeathAnimTime()
    {
        return deathAnimTime;
    }

    public float GetAttackAnimTime()
    {
        return attackAnimTime;
    }

    public float GetRangedAttackAnimTime()
    {
        return rangedAttackAnimTime;
    }

    public float GetSpecialAnimTime()
    {
        return specialAnimTime;
    }

    private void Awake()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
        if (BattleController.Instance.GetBattleState() == CurrentBattleState.Fight)
        {
            if (CharacterController.Instance.GetCharacterState() == CharacterState.Attack && !tookHit)
            {
                Debug.Log("Taking Hit");
                tookHit = true;
                MusicPlayer.Instance.PlaySoundEffectByName("EnemyHurt");
                this.currentHP -= (CharacterController.Instance.CurrentAttackDamage + UpgradeController.Instance.ExtraDamage);
                if (this.currentHP <= 0)
                {
                    GameController.Instance.ActivateEnemyAttackWarning(0);
                    this.currentHP = 0;
                    anim.Play("Death");
                    gameObject.GetComponent<BehaviorDesigner.Runtime.Behavior>().DisableBehavior(true);
                    StartCoroutine(EnemyDied());
                }
                if (this.currentHP > 0)
                {
                    anim.Play("TakeHit");
                }
                BattleController.Instance.UpdateHP(currentHP);
            }
            else if (CharacterController.Instance.GetCharacterState() != CharacterState.Attack)
            {
                tookHit = false;
            }
        }
    }

    public void SetAttackWarning(int timer)
    {
        GameController.Instance.ActivateEnemyAttackWarning(timer);
    }

    private IEnumerator EnemyDied()
    {
        MusicPlayer.Instance.PlaySoundEffectByName("EnemyDead");
        yield return new WaitForSeconds(deathAnimTime);
        SpawnController.Instance.SetEnemyOnScreen(false);
        BattleController.Instance.ClearEnemyPanel();
        BattleController.Instance.SetFightStarted(false);
        int tempBonus = Mathf.FloorToInt(UpgradeController.Instance.ExtraExp * level);
        if (tempBonus < 0)
        {
            tempBonus = 0;
        }
        //int tempExp = (exp + UpgradeController.Instance.ExtraExp) - ((CharacterController.Instance.Level - level)*2);
        int tempExp = (exp + tempBonus) - ((CharacterController.Instance.Level - level) * 2);
        if (tempExp < 0)
        {
            tempExp = 0;
        }
        CharacterController.Instance.GainExp(tempExp);
        BattleController.Instance.EndBattle();
        Destroy(gameObject);
    }
}
