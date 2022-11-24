using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class RangedAttack : EnemyAction
{
    [SerializeField] Enemy enemy;

    private float attackCooldownTime;
    private float attackCooldownTimer;

    private int damage;

    private bool hasAttacked;

    public override void OnStart()
    {
        if (enemy == null)
        {
            enemy = GetComponent<Enemy>();
        }

        attackCooldownTime = enemy.GetRangedAttackCooldown();

        damage = enemy.GetRangedDamage();

        GameController.Instance.SetScrollType(ScrollBackground.Static);
        BattleController.Instance.SetBattleState(CurrentBattleState.Ranged);
        BattleController.Instance.SetEnemyPanel(enemy.GetName(), enemy.GetRangedAttackType(), enemy.GetCurrentHP(), enemy.GetMaxHP(), enemy.GetAttackCooldown());
    }

    public override TaskStatus OnUpdate()
    {
        if (GameController.Instance.GetGameType() == GameType.Game)
        {
            attackCooldownTimer += Time.deltaTime;
            int timeLeft = Mathf.FloorToInt(attackCooldownTime - attackCooldownTimer) + 1;
            BattleController.Instance.UpdateAttackTime(timeLeft);
            enemy.SetAttackWarning(timeLeft);
            if (attackCooldownTimer >= attackCooldownTime)
            {
                MusicPlayer.Instance.PlaySoundEffectByName("ProjectileShoot");
                hasAttacked = true;
            }

            return hasAttacked ? TaskStatus.Success : TaskStatus.Running;
        }
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        anim.Play("RangedAttack");
        GameObject projectile = Object.Instantiate(enemy.GetProjectilePrefab(), enemy.GetProjectileSpawn().position, Quaternion.identity);
        projectile.GetComponent<Projectiles>().Damage = enemy.GetRangedDamage();
        BattleController.Instance.ClearEnemyPanel();
        hasAttacked = false;
        attackCooldownTimer = 0;
        anim.Play("Idle");
    }
}
