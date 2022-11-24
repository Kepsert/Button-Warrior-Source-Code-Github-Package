using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreButtons : GimmickButton
{
    int score;
    [SerializeField] int amountOfCoins = 3;
    [SerializeField] GameObject feedbackPrefab;
    [SerializeField] Transform feedbackSpawn;

    private void Start()
    {
        score = GameController.Instance.GetCurrentNumber();
    }

    public void ButtonPressed()
    {
        if (RhythmController.Instance.RhythmMode)
        {
            RhythmController.Instance.ChangeScore(Conductor.Instance.GiveScoreBasedOnTiming());
        }
        MusicPlayer.Instance.PlaySoundEffectByName("CoinPick");
        GameController.Instance.AddCoin(amountOfCoins);
        GameController.Instance.AddToScore(score * 10);
        GameController.Instance.AddToCurrentNumber();
        GameController.Instance.SpawnNewScoreButton();
        GameObject go = Instantiate(feedbackPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
        Destroy(gameObject);
    }

    private void Update()
    {
        SpawnButton();
        RemoveButton();
    }
}
