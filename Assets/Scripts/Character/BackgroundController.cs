using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public static BackgroundController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StopScroll()
    {
        GameController.Instance.SetScrollType(ScrollBackground.Static);
    }

    public void SlowScroll()
    {
        GameController.Instance.SetScrollType(ScrollBackground.Slow);
    }

    public void Scroll()
    {
        if (!BattleController.Instance.GetFightStarted() && GameController.Instance.GetGameType() != GameType.Victory)
        {
            GameController.Instance.SetScrollType(ScrollBackground.Scroll);
        }
        CharacterController.Instance.SetCharacterState(CharacterState.Walk);
    }
}
