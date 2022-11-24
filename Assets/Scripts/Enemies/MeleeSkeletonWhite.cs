using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkeletonWhite : EnemyDeprecated
{
    private void Update()
    {
        if (this.transform.position.x < -7.37f && this.transform.position.x > -7.47f)
        {
            GameController.Instance.SetScrollType(ScrollBackground.Static);
            SetState(BattleState.Fight);
            //BattleController.Instance.SetAttackTypeText("Block: Shield");
        }
    }
}
