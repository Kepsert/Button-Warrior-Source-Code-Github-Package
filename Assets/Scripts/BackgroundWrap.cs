using UnityEngine;

public class BackgroundWrap : MonoBehaviour
{
    [Range(-1f, 1f)]
    [SerializeField] float scrollSpeed = 0.5f;

    private float offset;
    private Material mat;

    [SerializeField] int sortingLayer;

    [SerializeField] int bgNumber;


    private void Start()
    {
        this.GetComponent<Renderer>().sortingLayerID = sortingLayer;
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (GameController.Instance.GetScrollType() == ScrollBackground.Scroll || GameController.Instance.GetScrollType() == ScrollBackground.Slow)
        {
            if (GameController.Instance.GetScrollType() == ScrollBackground.Scroll)
            {
                offset += (Time.deltaTime * scrollSpeed) / 10f;
            }
            if (GameController.Instance.GetScrollType() == ScrollBackground.Slow)
            {
                offset += (Time.deltaTime * (scrollSpeed/2)) / 10f;
            }
            mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }

        if (GameController.Instance.GetScrollType() == ScrollBackground.Static)
        {
            switch (bgNumber)
            {
                case 0: break;
                case 1:
                        offset += (Time.deltaTime * (scrollSpeed/3)) / 10f;
                    break;
                case 2:
                        offset += (Time.deltaTime * (scrollSpeed/3)) / 10f;
                    break;
            }
            mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }
}
