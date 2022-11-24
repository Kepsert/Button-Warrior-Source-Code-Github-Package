using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snek : EnemyDeprecated
{
    private void Update()
    {
        if (this.transform.position.x < -6.90f && this.transform.position.x > -7f)
        {
            GameController.Instance.SetScrollType(ScrollBackground.Static);
            SetState(BattleState.Fight);
            //BattleController.Instance.SetAttackTypeText("Block: Shield");
        }
    }
}
