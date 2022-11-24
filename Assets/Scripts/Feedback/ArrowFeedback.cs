using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowFeedback : MonoBehaviour
{
    [SerializeField] string direction;
    private void Awake()
    {
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(Move);
        sequence.AppendInterval(0.6f);
        sequence.AppendCallback(Remove);
    }

    private void Move()
    {
        switch (direction)
        {
            case "Up": this.transform.DOMove(new Vector3(this.transform.position.x, this.transform.position.y + 32), 0.4f).SetEase(Ease.OutQuad); break;
            case "Down": this.transform.DOMove(new Vector3(this.transform.position.x, this.transform.position.y - 32), 0.4f).SetEase(Ease.OutQuad); break;
            case "Left": this.transform.DOMove(new Vector3(this.transform.position.x - 32, this.transform.position.y), 0.4f).SetEase(Ease.OutQuad); break;
            case "Right": this.transform.DOMove(new Vector3(this.transform.position.x + 32, this.transform.position.y), 0.4f).SetEase(Ease.OutQuad); break;
        }
        
    }

    private void Remove()
    {
        Destroy(gameObject);
    }
}
