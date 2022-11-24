using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Obstacle : MonoBehaviour
{
    [Range(0, 1f)]
    [SerializeField] float scrollSpeed = 1f;

    private void FixedUpdate()
    {
        if (GameController.Instance.GetScrollType() == ScrollBackground.Scroll)
        {
            transform.Translate((Vector3.left * scrollSpeed * Time.deltaTime) / 1.75f);
        }
        if (this.transform.position.x < -3.5f)
        {
            Destroy(gameObject);
        }
    }
}
