using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Scroll, Fight, Ranged }
public enum AttackType { Shield, Jump, Slide }
public class EnemyDeprecated : MonoBehaviour
{
    [SerializeField] string name = "";

    [Range(0, 1f)]
    [SerializeField] float scrollSpeed = 1f;

    [SerializeField] int exp;
    [SerializeField] int maxHP = 5;
    [SerializeField] int currentHP;

    [SerializeField] int attackDamage = 6;

    [SerializeField] BattleState state = BattleState.Scroll;

    [SerializeField] Animator anim;

    [SerializeField] private float deathAnimTime;
    private float deathAnimTimer;
    private int attackTimer;

    [SerializeField] int attackInterval = 10;
    private float attackIntervalTimer;

    [SerializeField] int level;

    bool gotHit = false;
    bool dead = false;

    private bool battleStarted = false;

    private void Awake()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        currentHP = maxHP;
    }

    private void FixedUpdate()
    {
        if (GameController.Instance.GetScrollType() == ScrollBackground.Scroll)
        {
            transform.Translate(((Vector3.left * scrollSpeed * Time.deltaTime) / 1.7f)*1.33f);
        }
        if (this.transform.position.x < -8f)
        {
            Destroy(gameObject);
        }

        if (!dead && state == BattleState.Fight)
        {
            attackIntervalTimer += Time.deltaTime;
            if (attackIntervalTimer >= attackInterval)
            {
                anim.Play("Attack");
                if (CharacterController.Instance.GetCharacterState() != CharacterState.Shield && !BattleController.Instance.CanBlockHit())
                {
                    GameController.Instance.SetCurrentLives(attackDamage);
                }
                attackIntervalTimer = 0;
            }
        }

        if (dead)
        {
            deathAnimTimer += Time.deltaTime;
            if (deathAnimTimer > deathAnimTime)
            {
                CharacterController.Instance.GainExp((exp + UpgradeController.Instance.ExtraExp)-(CharacterController.Instance.Level - level));
                BattleController.Instance.EndBattle();
                Destroy(gameObject);
            }
        }

        if (state == BattleState.Fight)
        {
            if (CharacterController.Instance.GetCharacterState() == CharacterState.Attack)
            {
                if (!gotHit && !dead)
                {
                    this.currentHP -= (CharacterController.Instance.CurrentAttackDamage + UpgradeController.Instance.ExtraDamage);
                    if (this.currentHP < 0)
                    {
                        this.currentHP = 0;
                    }
                    BattleController.Instance.UpdateHP(this.currentHP);
                    if (this.currentHP > 0)
                    {
                        anim.Play("TakeHit");
                    }
                    else if (this.currentHP <= 0)
                    {
                        anim.Play("Death");
                        dead = true;
                    }
                    gotHit = true;
                }
            }
            else
            {
                gotHit = false;
            }
        }
    }

    public BattleState GetState()
    {
        return state;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

    public void SetState(BattleState state)
    {
        this.state = state;

        if (state == BattleState.Fight && !battleStarted)
        {
            battleStarted = true;
            BattleController.Instance.StartBattle(name, maxHP, currentHP, attackInterval);
        }
    }

    public void PlayAnim(string name)
    {
        anim.Play(name);
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }

    public string GetName()
    {
        return name;
    }
}
