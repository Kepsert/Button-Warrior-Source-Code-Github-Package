using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narrative : MonoBehaviour
{
    [SerializeField] string narrativeName;
    [SerializeField] string sceneToLoad;
    [SerializeField] string alternativeLevel;

    [System.Serializable]
    public struct NarrativeLine
    {
        public string name;
        [TextArea(4,8)]
        public string text;
        public float waitTime;
    }

    [SerializeField] NarrativeLine[] lines;

    private void Awake()
    {
        //if (!PlayerPrefs.HasKey(narrativeName))
        //{
            NarrativeController.Instance.StartNarrative(lines, sceneToLoad);
            PlayerPrefs.SetInt(narrativeName, 1);
        //}
        //else
        //{
        //    GameController.Instance.LoadScene(alternativeLevel);
        //}
    }
}
