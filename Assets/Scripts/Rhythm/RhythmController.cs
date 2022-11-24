using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmController : MonoBehaviour
{
    public static RhythmController Instance;

    [Range(0, 100)]
    private int rhythmScore = 70;

    private int streak;
    private int bonus;
    [SerializeField] int requiredStreak = 5;
    int currentRequiredStreak;

    [SerializeField] bool rhythmOn = true;

    [SerializeField] private Slider rhythmSlider;
    [SerializeField] private Image RhythmSliderFill;

    [SerializeField] Sprite redSlider;
    [SerializeField] Sprite orangeSlider;
    [SerializeField] Sprite greenSlider;

    [SerializeField] TextMeshProUGUI streakText;
    [SerializeField] TextMeshProUGUI bonusText;

    public bool rhythmMode { get; private set; }

    private void Awake()
    {
        Debug.Log(this.gameObject.name);
        if (Instance == null)
        {
            Instance = this;
        }

        if (rhythmOn)
        {
            rhythmMode = true;
        }
        else
        {
            rhythmMode = false;
        }
        currentRequiredStreak = requiredStreak;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (RhythmController.Instance.RhythmMode)
            {
                //Debug.Log("I got here");
                //RhythmController.Instance.ChangeScore(Conductor.Instance.GiveScoreBasedOnTiming());
            }
        }
    }

    public void Init()
    {
        rhythmSlider.value = 70;
        RhythmSliderFill.sprite = greenSlider;
        currentRequiredStreak = requiredStreak;
        streak = 0;
        bonus = 0;
    }

    public void ChangeScore(int score)
    {
        rhythmScore += score;

        if (rhythmScore > 100)
        {
            rhythmScore = 100;
        }
        if (rhythmScore < 0)
        {
            rhythmScore = 0;
        }
        rhythmSlider.value = rhythmScore;

        if (rhythmScore < 51 && rhythmScore > 20)
        {
            //Orange
            RhythmSliderFill.GetComponent<Image>().sprite = orangeSlider;
        }
        else if (rhythmScore < 21)
        {
            //Red
            RhythmSliderFill.GetComponent<Image>().sprite = redSlider;
        }
        else
        {
            //Green
            RhythmSliderFill.GetComponent<Image>().sprite = greenSlider;
        }

        if (rhythmScore < 20 && score < 0)
        {
            GameController.Instance.SetCurrentLives(2);
        }
        if (rhythmScore < 20 && score >= 0)
        {
            GameController.Instance.SetCurrentLives(1);
        }

        if (score >= 0)
        {
            streak++;
            bonus++;
        }
        else
        {
            streak = 0;
            bonus = 0;
        }

        if (bonus == currentRequiredStreak)
        {
            GameController.Instance.SpawnRandomButton();
            bonus = 0;
            currentRequiredStreak++;
        }

        if (streak > 0)
        {
            GameController.Instance.HitBeat(true);
        }
        if (streak == 0)
        {
            GameController.Instance.HitBeat(false);
        }

        streakText.text = "Streak: " + streak;
        bonusText.text = "Next Bonus: " + (currentRequiredStreak - bonus);
    }

    public bool RhythmMode
    {
        get { return rhythmMode; }
    }
}
