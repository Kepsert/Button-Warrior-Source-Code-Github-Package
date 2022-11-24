using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class WaitToFinishAttack : EnemyAction
{
    [SerializeField] Enemy enemy;

    private float attackTime;
    private float attackTimer;

    bool attackFinished = false;

    [SerializeField] CurrentBattleState endState;

    public override void OnStart()
    {
        if (enemy == null)
        {
            enemy = GetComponent<Enemy>();
        }

        attackTime = enemy.GetAttackAnimTime();
    }

    public override TaskStatus OnUpdate()
    {
        if (GameController.Instance.GetGameType() == GameType.Game)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackTime)
            {
                attackFinished = true;
            }
        }

        return attackFinished ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        BattleController.Instance.SetBattleState(endState);
        attackFinished = false;
        attackTimer = 0;
    }
}
