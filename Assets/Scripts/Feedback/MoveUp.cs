using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveUp : MonoBehaviour
{
    private void Awake()
    {
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(Move);
        sequence.AppendInterval(0.6f);
        sequence.AppendCallback(Remove);
    }

    private void Move()
    {
        this.transform.DOMove(new Vector3(this.transform.position.x, this.transform.position.y + 32), 0.4f).SetEase(Ease.OutQuad);
    }

    private void Remove()
    {
        Destroy(gameObject);
    }
}
