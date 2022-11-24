using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class SpecialAnim : EnemyAction
{
    [SerializeField] Enemy enemy;

    private bool specialFinished;

    private float specialTime;
    private float specialTimer;

    [SerializeField] string special;
    bool animPlayed = false;

    public override void OnStart()
    {
        if (enemy == null)
        {
            enemy = GetComponent<Enemy>();
        }

        specialTime = enemy.GetSpecialAnimTime();
    }

    public override TaskStatus OnUpdate()
    {
        if (GameController.Instance.GetGameType() == GameType.Game)
        {
            if (!animPlayed)
            {
                animPlayed = true;
                anim.Play(special);
            }
            specialTimer += Time.deltaTime;
            if (specialTimer > specialTime)
            {
                specialFinished = true;
            }

            return specialFinished ? TaskStatus.Success : TaskStatus.Running;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        specialFinished = false;
        specialTimer = 0f;
        animPlayed = false;
    }
}
