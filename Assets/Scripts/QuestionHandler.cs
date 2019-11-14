using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionHandler : MonoBehaviour
{
    public static QuestionHandler singleton;
    public GameObject quizPage;
    public List<Sprite> mathPictures;
    [SerializeField] private GameObject questionBoxWords, questionBoxPicture, correctResponseEffect;
    [SerializeField] private Slider timeBar;
    [SerializeField] private Image timerBarDisplay;
    [SerializeField] private Sprite mathTimerBar, engTimerBar;
    [SerializeField] private TextMeshProUGUI currQuestionNo, totalQuestions;
    private List<int> questionPool;
    private List<GameObject> targetChoices;
    private IDictionary<int, string> choicesRef;
    private IDictionary<int, bool> answerRef;
    private int currQn, totalQns, questionNo, realAns, score;
    private float maxTimer, currTimer;
    private GameObject questionBox;
    public bool timerRun;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        maxTimer = 32;
        questionPool = new List<int>();
        targetChoices = new List<GameObject>();
        choicesRef = new Dictionary<int, string>();
        answerRef = new Dictionary<int, bool>();
        timerRun = false;
    }

    public void beginQuiz()
    {
        quizPage.GetComponent<Animator>().Play("QuizPageStill");
        switch (BackendHandler.singleton.currSubject)
        {
            case BackendHandler.SUBJECTS.ENGLISH:
                questionBoxWords.SetActive(true);
                questionBoxPicture.SetActive(false);
                questionBox = questionBoxWords;
                timerBarDisplay.sprite = engTimerBar;
                break;

            case BackendHandler.SUBJECTS.MATH:
                questionBoxWords.SetActive(false);
                questionBoxPicture.SetActive(true);
                questionBox = questionBoxPicture;
                timerBarDisplay.sprite = mathTimerBar;
                break;
        }

        totalQns = LoadXMLFile.singleton.getNodeNumber(); //get all questions from XML file
        totalQuestions.text = "/" + totalQns;
        for (int i = 0; i < totalQns; i++)
        {
            questionPool.Add(i + 1);
        }
        currQn = 0;
        currTimer = maxTimer;
        timeBar.maxValue = currTimer;
        timeBar.value = currTimer;
        score = 0;
        questionBox.GetComponent<QuestionBoxHandler>().scoreDisplay.text = score.ToString();
        TargetSpawners.singleton.spawnTargets();
        nextQuestion();
    }

    public void nextQuestion()
    {
        answerRef.Clear();
        choicesRef.Clear();

        timerRun = false;

        currQn++;
        currQuestionNo.text = currQn.ToString();

        if (questionPool.Count > 1)
        {
            System.Random ran = new System.Random();
            int index = ran.Next(0, questionPool.Count);
            questionNo = questionPool[index];
            questionPool.RemoveAt(index);
        }
        else if (questionPool.Count == 1)
        {
            questionNo = questionPool[0];
            questionPool.Clear();
        }

        LoadXMLFile.singleton.label = "question" + questionNo.ToString();
        LoadXMLFile.singleton.updateQuestion();

        var qnHandler = questionBox.GetComponent<QuestionBoxHandler>();
        qnHandler.questionDisplay.text = LoadXMLFile.singleton.question;
        realAns = LoadXMLFile.singleton.answer;

        if (BackendHandler.singleton.currSubject == BackendHandler.SUBJECTS.MATH)
        {
            qnHandler.pictureDisplay.sprite = mathPictures[questionNo - 1];
        }

        targetChoices.Clear();

        string[] choices = LoadXMLFile.singleton.choices.Split('|');
        for (int i = 0; i < TargetSpawners.singleton.targetList.Count; i++)
        {
            var targetHandler = TargetSpawners.singleton.targetList[i].GetComponent<TargetHandler>();
            targetHandler.answerDisplay.text = choices[i];
            targetHandler.answer = false;
            targetChoices.Add(targetHandler.gameObject);
        }
        TargetSpawners.singleton.targetList[realAns - 1].GetComponent<TargetHandler>().answer = true;

        for (int n = 0; n < TargetSpawners.singleton.targetList.Count; n++)
        {
            System.Random ran2 = new System.Random();
            int index = ran2.Next(0, targetChoices.Count);
            choicesRef.Add(n, targetChoices[index].GetComponent<TargetHandler>().answerDisplay.text);
            answerRef.Add(n, targetChoices[index].GetComponent<TargetHandler>().answer);
            targetChoices.RemoveAt(index);
        }

        for (int b = 0; b < TargetSpawners.singleton.targetList.Count; b++)
        {
            var targetHandler = TargetSpawners.singleton.targetList[b].GetComponent<TargetHandler>();
            targetHandler.answerDisplay.text = choicesRef[b];
            targetHandler.answer = answerRef[b];
            TargetSpawners.singleton.targetList[b].GetComponent<Image>().color = Color.white;
            TargetSpawners.singleton.targetList[b].GetComponent<Button>().interactable = true;
            if (targetHandler.answer)
            {
                Debug.Log("answer is " + targetHandler.answerDisplay.text + ": " + TargetSpawners.singleton.targetList[b].name);
            }
        }

        timerRun = true;
    }

    public void answerToggle(bool correct) //trigged when user presses a button to answer
    {
        if (correct)
        {
            Debug.Log("correct answer");
            correctResponseEffect.SetActive(false);
            score++;
            questionBox.GetComponent<QuestionBoxHandler>().scoreDisplay.text = score.ToString();
        }

        if (currQn != totalQns)
        {
            TargetSpawners.singleton.spawnTargets();
            nextQuestion();
        }
        else
        {
            endQuiz(true);
        }
    }

    public void closeButtons() //disable buttons to prevent double responses to a question
    {
        foreach (var b in TargetSpawners.singleton.targetList)
        {
            b.GetComponent<Button>().interactable = false;
        }
    }

    public void showCorrectAnswer() //shows correct ans when user picks the wrong one
    {
        foreach (var b in TargetSpawners.singleton.targetList)
        {
            if (b.GetComponent<TargetHandler>().answer)
            {
                b.GetComponent<Image>().sprite = b.GetComponent<TargetHandler>().correctImg;
                b.GetComponent<TargetHandler>().resetImage();
            }
        }
    }

    public void correctResponse(GameObject target) //animation for correct answer
    {
        int layer = target.transform.GetSiblingIndex();
        correctResponseEffect.GetComponent<RectTransform>().anchoredPosition = target.GetComponent<RectTransform>().anchoredPosition;
        correctResponseEffect.transform.SetSiblingIndex(layer - 1);
        correctResponseEffect.SetActive(true);
        correctResponseEffect.GetComponent<Animator>().Play("CorrectTarget");
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
                endQuiz(false);
            }
        }
    }

    private void endQuiz(bool complete) //end of quiz
    {
        timerRun = false;
        questionPool.Clear();

        BackendHandler.totalScore += score;

        switch (BackendHandler.singleton.currSubject)
        {
            case BackendHandler.SUBJECTS.MATH:
                quizPage.SetActive(false);
                FinalPageHandler.singleton.mathScore.text = score.ToString();
                SubjectStartHandler.singleton.englishStartDisplay();
                break;
                
            case BackendHandler.SUBJECTS.ENGLISH:
                FinalPageHandler.singleton.englishScore.text = score.ToString();
                quizPage.GetComponent<Animator>().Play("QuizPageMove");
                FinalPageHandler.singleton.finalPageShow();
                break;
        }
    }
}
