using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopAndBop : EnemyDeprecated
{
    [SerializeField] bool canRangedAttack;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int rangedAttackDamage;

    [SerializeField] Transform projectileSpawn;

    [SerializeField] AttackType rangedAttackType = AttackType.Slide;

    [SerializeField] int rangedWaitTime = 5;
    float rangedWaitTimer;

    [SerializeField] float rangedAttackAnimTime;
    float rangedAttackAnimTimer;
    private bool setInfo = false;

    private bool shotRangedAttack;

    /*private void Start()
    {
        if (canRangedAttack)
        {
            switch (rangedAttackType)
            {
                case AttackType.Slide: BattleController.Instance.SetAttackTypeText("Dodge: Slide"); break;
                case AttackType.Shield: BattleController.Instance.SetAttackTypeText("Block: Shield"); break;
                case AttackType.Jump: BattleController.Instance.SetAttackTypeText("Dodge: Jump"); break;
            }
        }
    }*/

    string GetAttackType()
    {
        switch (rangedAttackType)
        {
            case AttackType.Slide: return "Dodge: Slide"; break;
            case AttackType.Shield: return "Block: Shield"; break;
            case AttackType.Jump: return "Dodge: Jump"; break;
        }
        return "";
    }

    private void Update()
    {
        if (canRangedAttack)
        {
            if (this.transform.position.x < -2.95f && this.transform.position.x > -3.05f)
            {
                BattleController.Instance.StartRangedAttack(GetAttackType(), GetName(), GetMaxHP(), rangedWaitTime);
                rangedWaitTimer += Time.deltaTime;
                GameController.Instance.SetScrollType(ScrollBackground.Static);
                SetState(BattleState.Ranged);
                if (rangedWaitTimer >= rangedWaitTime)
                {
                    PlayAnim("RangedAttack");
                    BattleController.Instance.EndRangedAttack();
                    GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
                    projectile.GetComponent<Projectiles>().Damage = rangedAttackDamage;
                    canRangedAttack = false;
                    shotRangedAttack = true;
                }
            }
        }

        if (GetState() == BattleState.Ranged && shotRangedAttack)
        {
            rangedAttackAnimTimer += Time.deltaTime;
            if (rangedAttackAnimTimer >= rangedAttackAnimTime)
            {
                SetState(BattleState.Scroll);
                GameController.Instance.SetScrollType(ScrollBackground.Scroll);
            }
        }

        if (this.transform.position.x < -6.6f && this.transform.position.x > -6.7)
        {
            GameController.Instance.SetScrollType(ScrollBackground.Static);
            SetState(BattleState.Fight);
            //BattleController.Instance.SetAttackTypeText("Block: Shield");
        }
    }
}
