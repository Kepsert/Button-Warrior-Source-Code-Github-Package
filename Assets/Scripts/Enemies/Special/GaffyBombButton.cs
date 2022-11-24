using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GaffyBombButton : GimmickButton
{
    [SerializeField] GameObject text;

    [SerializeField] private float bombTime = 4f;
    [SerializeField] private float bombTimer;

    [SerializeField] private int damage = 6;

    private void Start()
    {
        SpawnButton();
        bombTimer = bombTime;
    }

    public void Pressed()
    {
        text.SetActive(false);
        if (RhythmController.Instance.RhythmMode)
        {
            MusicPlayer.Instance.PlaySoundEffectByName("BombPick");
            RhythmController.Instance.ChangeScore(Conductor.Instance.GiveScoreBasedOnTiming());
        }
        Destroy(gameObject);
    }

    public void Unpressed()
    {
        text.SetActive(true);
    }

    private void Update()
    {
        if (GameController.Instance.GetGameType() == GameType.Game)
        {
            bombTimer -= Time.deltaTime;
        }

        text.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(bombTimer).ToString();

        if (bombTimer <= 0)
        {
            MusicPlayer.Instance.PlaySoundEffectByName("BombButtonExplode");
            GameController.Instance.SetCurrentLives(damage);
            Destroy(gameObject);
        }
    }
}
