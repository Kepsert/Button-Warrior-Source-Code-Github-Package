using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTapButton : GimmickButton
{
    [SerializeField] float doubleTapTime = 0.5f;
    float doubleTapTimer;
    bool tappedOnce = false;

    //[SerializeField] private int points = 10;

    private void Update()
    {
        if (tappedOnce)
        {
            doubleTapTimer += Time.deltaTime;
            if (doubleTapTimer >= doubleTapTime)
            {
                doubleTapTimer = 0;
                tappedOnce = false;
            }
        }

        SpawnButton();
        RemoveButton();
    }

    public void TappedButton()
    {
        if (tappedOnce)
        {
            if (RhythmController.Instance.RhythmMode)
            {
                RhythmController.Instance.ChangeScore(Conductor.Instance.GiveScoreBasedOnTiming());
            }
            MusicPlayer.Instance.PlaySoundEffectByName("LowerCoinCost");
            tappedOnce = false;
            AddToScore();
            FightShopController.Instance.RemoveFromCoinCost();
            Destroy(gameObject);
        }
        if (!tappedOnce)
        {
            MusicPlayer.Instance.PlaySoundEffectByName("ButtonPressed");
            tappedOnce = true;
        }
    }
}
