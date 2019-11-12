using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackendHandler : MonoBehaviour
{
    public static int totalScore;
    public static BackendHandler singleton;
    public enum SUBJECTS { MATH, ENGLISH };
    public SUBJECTS currSubject;
    public GameObject mainButton;
    [SerializeField] private GameObject countdownDisplay;
    public List<Sprite> buttonText, countdownImg;
    private int clickNo, countdownNo;
    private float clickTime, clickDelay;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        clickNo = 0;
        clickDelay = 0.5f;
        currSubject = SUBJECTS.MATH;
        totalScore = 0;
        countdownDisplay.transform.parent.gameObject.SetActive(false);
        SetButtonFunction("startGame");
    }

    //ref: https://forum.unity.com/threads/detect-double-click-on-something-what-is-the-best-way.476759/
    public void doubleClick()
    {
        clickNo++;
        if (clickNo == 1)
        {
            clickTime = Time.time;
        }

        if (clickNo > 1 && Time.time - clickTime < clickDelay)
        {
            clickNo = 0;
            clickTime = 0;
            SceneManager.LoadScene(0);
        }
        else if (clickNo > 2 || Time.time - clickTime > 1)
        {
            clickNo = 0;
        }
    }

    public void startGame()
    {
        StartPageHandler.singleton.playVideo();
        LoadXMLFile.singleton.changeXML("math");
        mainButton.SetActive(false);
    }

    public void startCountdown()
    {
        SubjectStartHandler.singleton.subjectStartPage.SetActive(false);
        countdownNo = 4;
        countdownDisplay.GetComponent<Image>().sprite = countdownImg[countdownNo];
        countdownDisplay.transform.parent.gameObject.SetActive(true);
        mainButton.SetActive(false);
        StartCoroutine(countLoop(1.6f));
    }

    private void toggleCountdown()
    {
        if (countdownNo > 1)
        {
            countdownNo--;
            countdownDisplay.GetComponent<Image>().sprite = countdownImg[countdownNo];
            //countdown.GetComponent<Animator>().Play("Countdown");
            StartCoroutine(countLoop(1.6f));
        }
        else
        {
            //countdown.GetComponent<Animator>().Play("CountStart");
            beginQuiz();
        }
    }

    private IEnumerator countLoop(float waitTime)
    {
        if (countdownNo > 1)
        {
            //playCountdownBeep();
        }
        else
        {
            //playCountdownLast();
        }

        yield return new WaitForSeconds(waitTime);
        toggleCountdown();
    }

    public void beginQuiz()
    {
        countdownDisplay.transform.parent.gameObject.SetActive(false);
        QuestionHandler.singleton.quizPage.SetActive(true);
        QuestionHandler.singleton.beginQuiz();
    }

    public void SetButtonFunction(string pageType)
    {
        mainButton.GetComponent<Button>().onClick.RemoveAllListeners();
        switch (pageType)
        {
            case "startGame":
                mainButton.transform.GetChild(0).GetComponent<Image>().sprite = buttonText[0];
                mainButton.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(700, 72);
                mainButton.GetComponent<Button>().onClick.AddListener(delegate { startGame(); });
                break;

            case "mathStart":
                mainButton.transform.GetChild(0).GetComponent<Image>().sprite = buttonText[1];
                mainButton.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(410, 196);
                mainButton.GetComponent<Button>().onClick.AddListener(delegate { startCountdown(); });
                break;

            case "englishStart":
                mainButton.transform.GetChild(0).GetComponent<Image>().sprite = buttonText[2];
                mainButton.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(410, 196);
                mainButton.GetComponent<Button>().onClick.AddListener(delegate { startCountdown(); });
                break;
        }

        mainButton.SetActive(true);
    }
}
