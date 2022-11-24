using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Conductor : MonoBehaviour
{
    public static Conductor Instance;
    public float songBpm;
    float secPerBeat;
    float songPosition;
    float songPositionInBeats;
    float dspSongTime;
    public float firstBeatOffset;
    public AudioSource musicSource;
    public float beatsPerLoop;
    int completedLoops = 0;
    float loopPositionInBeats;
    float loopPositionInAnalog;
    private int perfectStreak = 0;
    int faultStreak = 0;

    [SerializeField] GameObject[] notes;
    private int index = 2;
    private int lastIndex;
    private bool pulsed = false;
    private bool oncePerPulse = false;

    [SerializeField] string songTitle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        lastIndex = index - 1;
        if (lastIndex < 0)
        {
            lastIndex = 3;
        }
    }

    private void Start()
    {
        //musicSource = GetComponent<AudioSource>();

        secPerBeat = 60f / songBpm;

        dspSongTime = (float)AudioSettings.dspTime;

        MusicPlayer.Instance.ChangeBgMusic("LevelSelectLoop", false);
        MusicPlayer.Instance.ChangeBgMusic(songTitle, false);

        //if (musicSource != null)
        //{
        //    musicSource.Play();
        //}
    }

    public void PauseMusic()
    {
        //musicSource.Pause();
        MusicPlayer.Instance.PauseMusic(true);
    }

    public void ResumeMusic()
    {
        MusicPlayer.Instance.PauseMusic(false);
        //musicSource.Play();
    }

    private void Update()
    {
        if (GameController.Instance.GetGameType() == GameType.Game)
        {
            songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

            songPositionInBeats = songPosition / secPerBeat;

            if (songPositionInBeats >= (completedLoops + 1) * (beatsPerLoop / 4))
            {
                completedLoops++;
            }
            loopPositionInBeats = songPositionInBeats - completedLoops * (beatsPerLoop / 4);

            if (loopPositionInBeats >= 0.94f && !pulsed)
            {
                PulseOnBeat();
                pulsed = true;
            }
            if (loopPositionInBeats < 0.5f)
            {
                pulsed = false;
            }

            //Debug.Log("Loop Position In Beats: " + loopPositionInBeats + " - SongPositionInBeats: " + songPositionInBeats);

            loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
        }
    }

    public int GiveScoreBasedOnTiming()
    {
        Debug.Log("Clicked on loopPositionToBeats: " + loopPositionInBeats);
            if (loopPositionInBeats <= 0.15f || loopPositionInBeats >= 0.85f)
            {
                //MusicPlayer.Instance.PlaySoundEffectByName("CoinButtonPressed");
                Debug.Log("3 points");
                faultStreak = 0;
                perfectStreak++;
                return 3;
            }
            else if ((loopPositionInBeats > 0.15f && loopPositionInBeats <= 0.3f) || (loopPositionInBeats < 0.85f && loopPositionInBeats >= 0.7f))
            {
                //MusicPlayer.Instance.PlaySoundEffectByName("Coinclick");
                Debug.Log("1 point");
                faultStreak = 0;
                return 1;
            }
            else if ((loopPositionInBeats > 0.3f && loopPositionInBeats <= 0.48f) || (loopPositionInBeats < 0.7f && loopPositionInBeats >= 0.52f))
            {
                //MusicPlayer.Instance.PlaySoundEffectByName("Coinclick");
                Debug.Log("0 points");
                faultStreak = 0;
                return 0;
            }
            else
            {
                MusicPlayer.Instance.PlaySoundEffectByName("MissedBeat");
                faultStreak++;
                if (faultStreak == 0 || faultStreak == 1)
                {
                    return -1;
                }
                else
                {
                    return -1 - faultStreak;
                }
            }
    }

    public void PulseOnBeat()
    {
        GameController.Instance.PulseNotes();

        notes[index].transform.DOScale(new Vector3(.3f, .3f, 0), 0.1f).SetEase(Ease.InOutBounce);
        index++;

        notes[lastIndex].transform.DOScale(new Vector3(0.2f, 0.2f, 0), 0.1f).SetEase(Ease.InCirc);
        lastIndex++;
        if (lastIndex > 3)
        {
            lastIndex = 0;
        }

        if (index > 3)
        {
            index = 0;
        }


    }
}
