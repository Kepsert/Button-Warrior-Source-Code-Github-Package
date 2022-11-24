using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class James : EnemyDeprecated
{
    private void Update()
    {
        if (this.transform.position.x < -6.95f && this.transform.position.x > -7.05f)
        {
            GameController.Instance.SetScrollType(ScrollBackground.Static);
            SetState(BattleState.Fight);
            //BattleController.Instance.SetAttackTypeText("Block: Shield");
        }
    }
}
