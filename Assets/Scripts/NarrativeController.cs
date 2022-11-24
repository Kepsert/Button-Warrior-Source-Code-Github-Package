using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NarrativeController : MonoBehaviour
{
    public static NarrativeController Instance;

    [SerializeField] GameObject narrativeFade;
    [SerializeField] Animator fadeScreenAnim;
    [SerializeField] GameObject narrativeFadeText;
    [SerializeField] Animator fadeTextAnim;
    [SerializeField] TextMeshProUGUI narrativeText;

    [SerializeField] GameObject skipButton;

    Narrative.NarrativeLine[] narrative;

    bool narrativeReceived;

    private float waitTime;
    private float waitTimer;

    private int index = 0;

    private string sceneToLoad;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (fadeScreenAnim == null)
        {
            fadeScreenAnim = narrativeFade.GetComponent<Animator>();
        }

        sceneToLoad = "";
    }

    public void StartNarrative(Narrative.NarrativeLine[] lines, string sceneToLoad)
    {
        this.sceneToLoad = "";
        this.sceneToLoad = sceneToLoad;
        narrative = lines;
        narrativeReceived = true;
        StartCoroutine(FadeInNarrativeScreen());
    }

    IEnumerator FadeInNarrativeScreen()
    {
        fadeScreenAnim.SetBool("Fade", true);
        yield return new WaitForSeconds(2.5f);
        narrativeFadeText.SetActive(true);
        skipButton.SetActive(true);
        if (fadeTextAnim == null)
        {
            fadeTextAnim = narrativeFadeText.GetComponent<Animator>();
        }
        StartCoroutine(FadeInNarrativeText());
    }

    IEnumerator FadeInNarrativeText()
    {
        Debug.Log("Do I get here again and what index?: " + index);
        narrativeText.text = narrative[index].text;
        fadeTextAnim.SetBool("Fade", false);
        yield return new WaitForSeconds(narrative[index].waitTime + 1.5f);
        StartCoroutine(FadeOutNarrativeText());
    }

    IEnumerator FadeOutNarrativeText()
    {
        Debug.Log("Fading out text");
        fadeTextAnim.SetBool("Fade", true);
        index++;
        yield return new WaitForSeconds(2f);
        if (index >= narrative.Length)
        {
            narrativeFadeText.SetActive(false);
            skipButton.SetActive(false);
            index = 0;
            fadeScreenAnim.SetBool("Fade", false);
            if (sceneToLoad != "")
            {
                GameController.Instance.LoadScene(sceneToLoad);
            }
            sceneToLoad = "";
        }
        else
        {
            Debug.Log("Will I fade in next text?");
            StartCoroutine(FadeInNarrativeText());
        }
    }

    public void SkipNarrative()
    {
        StopAllCoroutines();
        StartCoroutine(SkipNarrativeTime());
        fadeScreenAnim.SetBool("Fade", false);
        fadeTextAnim.SetBool("Fade", true);
        skipButton.SetActive(false);
        if (sceneToLoad != "")
        {
            GameController.Instance.LoadScene(sceneToLoad);
            //SceneManager.LoadScene(sceneToLoad);
        }
        sceneToLoad = "";
    }

    IEnumerator SkipNarrativeTime()
    {
        yield return new WaitForSeconds(2f);
        index = 0;
        narrativeFadeText.SetActive(false);
    }
}
