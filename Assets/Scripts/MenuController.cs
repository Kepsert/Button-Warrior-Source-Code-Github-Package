using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    [SerializeField] GameObject mainManuPanel;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject menuPlayPanel;
    [SerializeField] GameObject newGameDialog;
    [SerializeField] GameObject loadButton;
    [SerializeField] Toggle windowedToggle;
    [SerializeField] GameObject resolutionToggles;
    [SerializeField] Toggle res1Toggle;
    [SerializeField] Toggle res2Toggle;
    [SerializeField] Toggle res3Toggle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Screen.fullScreen = !Screen.fullScreen;
        

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            resolutionToggles.SetActive(false);
            Screen.SetResolution(360, 720, false, 60);
        }
        else
        {
            Screen.SetResolution(450, 900, false, 60);
        }
        if (!PlayerPrefs.HasKey("SaveFile"))
        {
            loadButton.GetComponent<Button>().interactable = false;
        }
    }

    public void NewPressed(bool noDialogButton)
    {
        if (PlayerPrefs.HasKey("SaveFile") && noDialogButton)
        {
            newGameDialog.SetActive(true);
        }
        else
        {
            float tempMusic = PlayerPrefs.GetFloat("MusicVolume");
            float tempSfx = PlayerPrefs.GetFloat("SFXVolume");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetFloat("MusicVolume", tempMusic);
            PlayerPrefs.SetFloat("SFXVolume", tempSfx);

            UpgradeController.Instance.NewGame();
            PlayerPrefs.DeleteKey(("LastLevelBeaten"));
            GameController.Instance.Init();
            LevelController.Instance.UpdateLevelSelect();
            SceneManager.LoadScene("Narrative1");
            StartCoroutine(ClearMainMenu());
            PlayerPrefs.SetInt("SaveFile", 1);
        }
    }

    public void LoadPressed()
    {
        GameController.Instance.LoadScene("LevelSelectScene");
        StartCoroutine(ClearMainMenu());
    }

    public void ClearMenu()
    {
        StartCoroutine(ClearMainMenu());
    }

    IEnumerator ClearMainMenu()
    {
        yield return new WaitForSeconds(3f);
        mainManuPanel.SetActive(false);
        mainPanel.SetActive(true);
        menuPlayPanel.SetActive(false);
    }

    public void SetWindowed()
    {
        if (windowedToggle.isOn)
        {
            Screen.fullScreen = !Screen.fullScreen;
            SetResolution();
            resolutionToggles.SetActive(true);
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
            resolutionToggles.SetActive(false);
        }
    }

    public void SetResolution()
    {
        if (res1Toggle.isOn)
        {
            Screen.SetResolution(360, 720, false, 60);
        }
        else if (res2Toggle.isOn)
        {
            Screen.SetResolution(450, 900, false, 60);
        }
        else if (res3Toggle.isOn)
        {
            Screen.SetResolution(540, 1080, false, 60);
        }
    }

    public void PlayButtonSFX(string name)
    {
        MusicPlayer.Instance.PlaySoundEffectByName(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
