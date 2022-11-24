using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public enum Direction { Up, Down, Left, Right }
public class SwipeButton : GimmickButton
{
    [SerializeField] private Direction dir = Direction.Up;

    private Vector2 startPos;
    [SerializeField] int pixelDistToDetect = 80;
    private bool fingerDown;

    [SerializeField] GameObject arrow;
    private bool isArrowActive = false;
    private bool userFeedback = false;
    private float arrowTimer = 0.25f;
    float arrowTime;

    [SerializeField] float arrowDistance = 30;

    int feedbackCount = 0;

    private bool killSequence;

    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform arrowSpawn;

    //[SerializeField] int points = 10;

    private void Awake()
    {
        /*int temp = Random.Range(0, 4);
        switch (temp)
        {
            case 0: dir = Direction.Up; break;
            case 1: dir = Direction.Down; break;
            case 2: dir = Direction.Left; break;
            case 3: dir = Direction.Right; break;
        }*/
    }

    void Start()
    {
        switch (dir)
        {
            //case Direction.Up: GetComponentInChildren<TextMeshProUGUI>().text = "Swipe Up"; break;
            //case Direction.Down: GetComponentInChildren<TextMeshProUGUI>().text = "Swipe Down"; break;
            //case Direction.Left: GetComponentInChildren<TextMeshProUGUI>().text = "Swipe Left"; break;
            //case Direction.Right: GetComponentInChildren<TextMeshProUGUI>().text = "Swipe Right"; break;
        }
    }

    private void Update()
    {
        SpawnButton();
        RemoveButton();

        if (isArrowActive)
        {
            if (feedbackCount >= 6)
            {
                GameController.Instance.SetFeedback(Feedback.Off);
            }
            userFeedback = true;

            Debug.Log("Spawning feedback");
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity, this.transform.parent);
            isArrowActive = false;
        }

        
    }

    private void MoveFeedbackArrow()
    {
        switch (dir)
        {
            case Direction.Up:
                arrow.transform.DOMove(new Vector3(arrow.transform.position.x, arrow.transform.position.y + arrowDistance, 0), arrowTimer).SetEase(Ease.OutQuad);
                break;
            case Direction.Down:
                arrow.transform.DOMove(new Vector3(arrow.transform.position.x, arrow.transform.position.y - arrowDistance, 0), arrowTimer).SetEase(Ease.OutQuad);
                break;
            case Direction.Left:
                arrow.transform.DOMove(new Vector3(arrow.transform.position.x - arrowDistance, arrow.transform.position.y, 0), arrowTimer).SetEase(Ease.OutQuad);
                break;
            case Direction.Right:
                arrow.transform.DOMove(new Vector3(arrow.transform.position.x + arrowDistance, arrow.transform.position.y, 0), arrowTimer).SetEase(Ease.OutQuad);
                break;
        }
    }

    private void RemoveFeedback()
    {

        arrow.SetActive(false);
    }

    public void PressButton()
    {
        
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (!fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                
                startPos = Input.touches[0].position;
                fingerDown = true;
                if (!userFeedback && GameController.Instance.GetFeedback() == Feedback.On)
                {
                    GameController.Instance.AddFeedback();
                    isArrowActive = true;
                }
            }
        }
        else
        {
            if (!fingerDown && Input.GetMouseButtonDown(0))
            {
                
                startPos = Input.mousePosition;
                fingerDown = true;
                if (!userFeedback && GameController.Instance.GetFeedback() == Feedback.On)
                {
                    GameController.Instance.AddFeedback();
                    isArrowActive = true;
                }
            }
        }
    }

    public void ReleaseButton()
    {
        if (fingerDown)
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                switch (dir)
                {
                    case Direction.Up:
                        if (Input.touches[0].position.y >= startPos.y + pixelDistToDetect)
                        {
                            if (CharacterController.Instance.GetCharacterState() != CharacterState.Dead)
                            {
                                CharacterController.Instance.PlayAnim("Warrior_Jump");
                            }
                            MusicPlayer.Instance.PlaySoundEffectByName("Jumped");
                            CorrectSwipe();
                        }
                        else
                        {
                            Debug.Log("Incorrect Swipe!");
                        }
                        break;
                    case Direction.Down:
                        if (Input.touches[0].position.y <= startPos.y + pixelDistToDetect)
                        {
                            if (CharacterController.Instance.GetCharacterState() != CharacterState.Dead)
                            {
                                CharacterController.Instance.PlayAnim("Warrior_Slide");
                            }
                            MusicPlayer.Instance.PlaySoundEffectByName("Slide");
                            CorrectSwipe();
                        }
                        else
                        {
                            Debug.Log("Incorrect Swipe!");
                        }
                        break;
                    case Direction.Left:
                        if (Input.touches[0].position.x <= startPos.x + pixelDistToDetect)
                        {
                            if (CharacterController.Instance.GetCharacterState() != CharacterState.Dead)
                            {
                                CharacterController.Instance.PlayAnim("Warrior_Shield");
                            }
                            MusicPlayer.Instance.PlaySoundEffectByName("ShieldUp");
                            CorrectSwipe();
                        }
                        else
                        {
                            Debug.Log("Incorrect Swipe!");
                        }
                        break;
                    case Direction.Right:
                        if (Input.touches[0].position.x >= startPos.x + pixelDistToDetect)
                        {
                            if (CharacterController.Instance.GetCharacterState() != CharacterState.Dead)
                            {
                                CharacterController.Instance.PlayAnim("Warrior_Slash");
                            }
                            //MusicPlayer.Instance.PlaySoundEffectByName("Slide");
                            CorrectSwipe();
                        }
                        else
                        {
                            Debug.Log("Incorrect Swipe!");
                        }
                        break;
                }
            }
            else
            {
                switch (dir)
                {
                    case Direction.Up:
                        if (Input.mousePosition.y >= startPos.y + pixelDistToDetect)
                        {
                            if (CharacterController.Instance.GetCharacterState() != CharacterState.Dead)
                            {
                                CharacterController.Instance.PlayAnim("Warrior_Jump");
                            }
                            MusicPlayer.Instance.PlaySoundEffectByName("Jumped");
                            CorrectSwipe();
                        }
                        else
                        {
                            Debug.Log("Incorrect Swipe!");
                        }
                        break;
                    case Direction.Down:
                        if (Input.mousePosition.y <= startPos.y - pixelDistToDetect)
                        {
                            if (CharacterController.Instance.GetCharacterState() != CharacterState.Dead)
                            {
                                CharacterController.Instance.PlayAnim("Warrior_Slide");
                            }
                            MusicPlayer.Instance.PlaySoundEffectByName("Slide");
                            CorrectSwipe();
                        }
                        else
                        {
                            Debug.Log("Incorrect Swipe!");
                        }
                        break;
                    case Direction.Left:
                        if (Input.mousePosition.x <= startPos.x - pixelDistToDetect)
                        {
                            if (CharacterController.Instance.GetCharacterState() != CharacterState.Dead)
                            {
                                CharacterController.Instance.PlayAnim("Warrior_Shield");
                            }
                            MusicPlayer.Instance.PlaySoundEffectByName("ShieldUp");
                            CorrectSwipe();
                        }
                        else
                        {
                            Debug.Log("Incorrect Swipe!");
                        }
                        break;
                    case Direction.Right:
                        if (Input.mousePosition.x >= startPos.x + pixelDistToDetect)
                        {
                            if (CharacterController.Instance.GetCharacterState() != CharacterState.Dead)
                            {
                                CharacterController.Instance.PlayAnim("Warrior_Slash");
                            }
                            //MusicPlayer.Instance.PlaySoundEffectByName("Slide");
                            CorrectSwipe();
                        }
                        else
                        {
                            Debug.Log("Incorrect Swipe!");
                        }
                        break;
                }
            }
        }
        fingerDown = false;
    }

    private void CorrectSwipe()
    {
        AddToScore();
        fingerDown = false;
        if (RhythmController.Instance.RhythmMode)
        {
            RhythmController.Instance.ChangeScore(Conductor.Instance.GiveScoreBasedOnTiming());
        }
        Destroy(gameObject);
    }
}
