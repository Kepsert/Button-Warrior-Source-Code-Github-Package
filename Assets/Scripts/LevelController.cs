using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    [SerializeField] List<Button> levels = new List<Button>();
    [SerializeField] List<GameObject> levelText = new List<GameObject>();
    int lastLevelBeaten;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }

    private void Start()
    {
        UpdateLevelSelect();
    }

    public void UpdateLevelSelect()
    {
        foreach (Button button in levels)
        {
            button.interactable = false;
        }
        foreach (GameObject go in levelText)
        {
            go.SetActive(false);
        }
        lastLevelBeaten = GameController.Instance.GetLastLevelBeaten();
        Debug.Log("Last Level Beaten: " + lastLevelBeaten);

        for (int i = 0; i < lastLevelBeaten+1; i++)
        {
            levels[i].interactable = true;
            levels[i].enabled = true;
            levels[i].gameObject.GetComponentInChildren<RectTransform>().gameObject.SetActive(true);
            if (levels[i].interactable)
            {
                levelText[i].SetActive(true);
            }
        }
    }

    public void LoadLevel(string name)
    {
        GameController.Instance.LoadScene(name);
    }
}
