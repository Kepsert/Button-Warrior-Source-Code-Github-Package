using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpike : Obstacle
{
    [SerializeField] private int baseDamage;

    private bool damageDealt;

    private void Update()
    {
        if (!damageDealt)
        {
            if (this.transform.position.x < -2f && this.transform.position.x > -2.5f)
            {
                Debug.Log("Within Range");
                if (CharacterController.Instance.GetCharacterState() != CharacterState.Jump)
                {
                    GameController.Instance.ChangeHP(-baseDamage);
                    damageDealt = true;
                }
            }
        }
    }
}
