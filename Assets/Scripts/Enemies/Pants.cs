using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pants : EnemyDeprecated
{
    private void Update()
    {
        if (this.transform.position.x < -6.8f && this.transform.position.x > -6.9f)
        {
            GameController.Instance.SetScrollType(ScrollBackground.Static);
            SetState(BattleState.Fight);
            //BattleController.Instance.SetAttackTypeText("Block: Shield");
        }
    }
}
