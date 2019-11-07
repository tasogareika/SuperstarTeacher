using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackendHandler : MonoBehaviour
{
    public static BackendHandler singleton;
    public enum SUBJECTS { MATH, ENGLISH };
    public SUBJECTS currSubject;
    public GameObject mainButton;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        currSubject = SUBJECTS.MATH;
        SetButtonFunction("startGame");
    }

    public void startGame()
    {
        StartPageHandler.singleton.playVideo();
        mainButton.SetActive(false);
    }

    public void beginQuiz()
    {
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
    }
}
