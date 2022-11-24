using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class MeleeAttack : EnemyAction
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

        attackCooldownTime = enemy.GetAttackCooldown();

        damage = enemy.GetDamage();

        GameController.Instance.SetScrollType(ScrollBackground.Static);
        BattleController.Instance.SetBattleState(CurrentBattleState.Fight);
        BattleController.Instance.SetEnemyPanel(enemy.GetName(), "Block: Shield", enemy.GetCurrentHP(), enemy.GetMaxHP(), enemy.GetAttackCooldown());
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
                anim.Play("Attack");
                if (CharacterController.Instance.GetCharacterState() != CharacterState.Shield && !BattleController.Instance.CanBlockHit())
                {
                    MusicPlayer.Instance.PlaySoundEffectByName("ShieldBlock");
                    GameController.Instance.SetCurrentLives(damage);
                }
                hasAttacked = true;
            }

            return hasAttacked ? TaskStatus.Success : TaskStatus.Running;
        }
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        BattleController.Instance.SetBattleState(CurrentBattleState.Clear);
        BattleController.Instance.ClearEnemyPanel();
        hasAttacked = false;
        attackCooldownTimer = 0;
        anim.Play("Idle");
    }
}
