using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    
    [SerializeField] float movementSpeed;
    int damage { get; set; }

    bool hasPassed;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private void Update()
    {
        transform.Translate((Vector3.left * movementSpeed * Time.deltaTime) / 1.9f);

        if (!hasPassed)
        {
            if (this.transform.position.x <= -1.9f)
            {
                if (this.transform.position.y >= 3)
                {
                    if (CharacterController.Instance.GetCharacterState() != CharacterState.Slide && !BattleController.Instance.CanBlockHit())
                    {
                        GameController.Instance.SetCurrentLives(damage);
                    }
                }
                else
                {
                    if (CharacterController.Instance.GetCharacterState() != CharacterState.Jump && !BattleController.Instance.CanBlockHit())
                    {
                        GameController.Instance.SetCurrentLives(damage);
                    }
                }
                hasPassed = true;
            }
        }

        if (this.transform.position.x <= -13f)
        {
            Destroy(gameObject);
        }
    }
}
