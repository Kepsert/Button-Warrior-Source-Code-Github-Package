using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoldButton : GimmickButton
{
    private bool buttonHeld;
    [SerializeField] private float heldTime = 2f;
    private float heldTimer;

    private void Awake()
    {
        //GetComponentInChildren<TextMeshProUGUI>().text = "Hold";
    }

    private void Update()
    {
        if (buttonHeld)
        {
            heldTimer += Time.deltaTime;

            if (heldTimer >= heldTime)
            {
                //CharacterController.Instance.PlayAnim("Warrior_Walk");
                //AddToScore();
                //Destroy(gameObject); 
                ButtonReleased();
            }
        }
        SpawnButton();
        RemoveButton();
    }

    public void ButtonPressed()
    {
        if (CharacterController.Instance.GetCharacterState() == CharacterState.Walk)
        {
            buttonHeld = true;
            CharacterController.Instance.PlayAnim("Warrior_Shield");
            GameController.Instance.SetScrollType(ScrollBackground.Static);
        }
    }

    public void ButtonReleased()
    {
        if (buttonHeld)
        {
            CharacterController.Instance.PlayAnim("Warrior_Walk");
            CharacterController.Instance.SetCharacterState(CharacterState.Walk);
            buttonHeld = false;
        }
        AddToScore();
        Destroy(gameObject);
    }
}
