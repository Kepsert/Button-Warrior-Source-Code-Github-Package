using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ScrollToPlayer : EnemyAction
{
    [SerializeField] float minDistance, maxDistance;
    [SerializeField] float speed = 1;

    [SerializeField] bool moveNoScroll = false;
    [SerializeField] bool hasWalkAnim = false;

    private bool inPosition;

    public override TaskStatus OnUpdate()
    {
        if (GameController.Instance.GetGameType() == GameType.Game)
        {
            if (!moveNoScroll)
            {
                GameController.Instance.SetScrollType(ScrollBackground.Scroll);
            }
            if (GameController.Instance.GetScrollType() == ScrollBackground.Scroll || moveNoScroll)
            {
                if (hasWalkAnim)
                {
                    anim.Play("Walk");
                }
                transform.Translate(((Vector3.left * speed * Time.deltaTime) / 1.7f) * 1.33f);
            }

            if (this.transform.position.x < minDistance && this.transform.position.x > maxDistance)
            {
                BattleController.Instance.SetFightStarted(true);
                inPosition = true;
            }
        }

        return inPosition ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        if (hasWalkAnim)
        {
            anim.Play("Idle");
        }
        inPosition = false;
    }
}
