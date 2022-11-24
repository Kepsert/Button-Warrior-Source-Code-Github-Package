using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasEmpty : MonoBehaviour
{
    public static CanvasEmpty Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private CanvasScaler canvasScaler;

    private float ScreenCanvasRatio
    {
        get
        {
            if (canvasScaler == null)
            {
                canvasScaler = GameObject.FindObjectOfType<CanvasScaler>();
            }

            if (canvasScaler)
            {
                return Screen.width / canvasScaler.referenceResolution.x;
            }
            else
            {
                return 1;
            }
        }
    }

    public Vector3 GetRectTransformFitSize(RectTransform rt)
    {
        Vector3 newVector = new Vector3();

        newVector.x = rt.rect.size.x * this.ScreenCanvasRatio;
        newVector.y = rt.rect.size.y * this.ScreenCanvasRatio;
        return newVector;
    }
}
