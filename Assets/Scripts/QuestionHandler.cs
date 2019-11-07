using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionHandler : MonoBehaviour
{
    public static QuestionHandler singleton;
    public GameObject quizPage;
    [SerializeField] private GameObject questionBoxWords, questionBoxPicture;
    [SerializeField] private Slider timeBar;
    [SerializeField] private TextMeshProUGUI currQuestionNo, totalQuestions;
    private List<int> questionPool;
    private int currQn, totalQns;
    private float maxTimer, currTimer;
    private bool timerRun;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        maxTimer = 60;
        timerRun = false;
    }

    public void beginQuiz()
    {
        totalQns = LoadXMLFile.singleton.getNodeNumber() + 1; //get all questions from XML file
        totalQuestions.text = "/" + totalQns;
        currQn = 1;
        currQuestionNo.text = currQn.ToString();
        currTimer = maxTimer;
        timeBar.maxValue = currTimer;
        timeBar.value = currTimer;
        timerRun = true;
    }

    private void Update()
    {
        if (timerRun)
        {
            if (currTimer > 0)
            {
                currTimer -= Time.deltaTime;
                timeBar.value = currTimer;
            }
            else
            {
                //time's up
                timerRun = false;
            }
        }
    }
}
