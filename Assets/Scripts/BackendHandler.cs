using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackendHandler : MonoBehaviour
{
    public static BackendHandler singleton;
    public enum SUBJECTS { MATH, ENGLISH };
    public SUBJECTS currSubject;
    public GameObject mainButton;
    private int clickNo;
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

    public void beginQuiz()
    {
        mainButton.SetActive(false);
        SubjectStartHandler.singleton.subjectStartPage.SetActive(false);
        QuestionHandler.singleton.quizPage.SetActive(true);
        QuestionHandler.singleton.beginQuiz();
    }

    public void SetButtonFunction(string pageType)
    {
        mainButton.GetComponent<Button>().onClick.RemoveAllListeners();
        switch (pageType)
        {
            case "startGame":
                mainButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Start Game";
                mainButton.GetComponent<Button>().onClick.AddListener(delegate { startGame(); });
                break;

            case "mathStart":
                mainButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Move to Math";
                mainButton.GetComponent<Button>().onClick.AddListener(delegate { beginQuiz(); });
                break;
        }

        mainButton.SetActive(true);
    }
}
