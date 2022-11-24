
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GimmickButton : MonoBehaviour
{
    [SerializeField] public int points;

    [SerializeField] private float clearTime = 10f;
    [SerializeField] private float clearTimer;

    private Vector3 originalSize;
    private Vector3 scaleTo;

    private bool Spawned = false;
    
    public void SpawnButton()
    {
        if (!Spawned)
        {
            Spawned = true;
            originalSize = this.transform.localScale;
            scaleTo = originalSize * 8;

            transform.DOScale(scaleTo, .1f).SetEase(Ease.InCirc);
        }
        
    }

    public void RemoveButton()
    {
        clearTimer += Time.deltaTime;
        if (clearTimer >= clearTime && !this.name.StartsWith("ScoreButton"))
        {
            //GIVE USER FEEDBACK
            //GameController.Instance.SetCurrentLives(1);
            //Destroy(gameObject);
        }
    }

    public void AddToScore()
    {
        GameController.Instance.AddToScore(points*GameController.Instance.GetCurrentLevel());
    }
}
