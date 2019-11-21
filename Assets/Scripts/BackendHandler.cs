using TMPro;
using System.IO;
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
    [HideInInspector] public string pathFile;
    private int clickNo, countdownNo;
    private float clickTime, clickDelay, time;
    [HideInInspector] public Animator buttonAnimator, countDownAnimator;
    private AudioSource audioPlayer, SFXPlayer;
    public AudioClip titleBGM, gameBGM, buttonClickSFX, countdownSFX, countdownFinalSFX, correctSFX, wrongSFX;

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
        buttonAnimator = mainButton.GetComponent<Animator>();
        countDownAnimator = countdownDisplay.transform.parent.GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        SFXPlayer = Camera.main.gameObject.GetComponent<AudioSource>();

        pathFile = Application.dataPath + "/" + "scoreplayed.txt";
        if (!File.Exists(pathFile))
        {
            StreamWriter writer = new StreamWriter(pathFile, true);
            writer.WriteLine("High Score:0\nTimes Played:0");
            writer.Close();
        }

        //set portrait res
        #if UNITY_STANDALONE
        //Screen.SetResolution(720, 1280, false);
        Screen.SetResolution(1080, 1920, true);
        #endif
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

    public void mainButtonShow()
    {
        buttonAnimator.enabled = true;
        buttonAnimator.Play("MainButtonMove");
    }

    public void mainButtonReturn()
    {
        buttonAnimator.enabled = true;
        buttonAnimator.Play("MainButtonMoveback");
    }

    public void startGame()
    {
        SetButtonFunction("clear");
        playSFX(1);
        StartPageHandler.singleton.shiftToVideo();
        LoadXMLFile.singleton.changeXML("math");
    }

    private void shiftToCountDown()
    {
        SubjectStartHandler.singleton.moveToCountdown();
        mainButtonShow();
        playSFX(1);
        countdownDisplay.transform.parent.gameObject.SetActive(true);
        countDownAnimator.Play("CountdownAppear");
        StartCoroutine(beginCountdown(getAnimTime(countDownAnimator) + 0.2f));
    }

    public void startCountdown()
    {
        SubjectStartHandler.singleton.subjectStartPage.SetActive(false);
        countdownNo = 4;
        countdownDisplay.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        toggleCountdown();
        mainButton.SetActive(false);
    }

    private void toggleCountdown()
    {
        if (countdownNo >= 1)
        {
            countdownNo--;
            countdownDisplay.GetComponent<Image>().sprite = countdownImg[countdownNo];
            countDownAnimator.Play("Countdown");
            StartCoroutine(countLoop(1.5f));
        }
        else
        {
            beginQuiz();
        }

        if (countdownNo == 0)
        {
            countdownDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 120f);
        }
    }

    private IEnumerator beginCountdown (float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        startCountdown();
    }

    private IEnumerator countLoop(float waitTime)
    {
        if (countdownNo >= 1)
        {
            playSFX(2);
        }
        else
        {
            playSFX(3);
        }

        yield return new WaitForSeconds(waitTime);
        toggleCountdown();
    }

    public void playSFX (int SFXNo)
    {
        switch (SFXNo)
        {
            case 1:
                SFXPlayer.clip = buttonClickSFX;
                break;

            case 2:
                SFXPlayer.clip = countdownSFX;
                break;

            case 3:
                SFXPlayer.clip = countdownFinalSFX;
                break;

            case 4:
                SFXPlayer.clip = correctSFX;
                break;

            case 5:
                SFXPlayer.clip = wrongSFX;
                break;
        }

        SFXPlayer.Play();
    }

    public void playBGM (string track)
    {
        stopBGM();

        switch (track)
        {
            case "titleBGM":
                audioPlayer.clip = titleBGM;
                break;

            case "gameBGM":
                audioPlayer.clip = gameBGM;
                break;
        }

        audioPlayer.Play();
    }

    public void stopBGM()
    {
        audioPlayer.Stop();
    }

    public float getAnimTime(Animator anim)
    {
        float returnTime;
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        for (var i = 0; i < clips.Length; i++)
        {
            switch (clips[i].name)
            {
                case "StartPageAppear":
                    time = clips[i].length;
                    break;

                case "StartPageMove":
                    time = clips[i].length;
                    break;

                case "VideoMove":
                    time = clips[i].length;
                    break;

                case "SubjectAppear":
                    time = clips[i].length;
                    break;

                case "SubjectPopMath":
                    time = clips[i].length;
                    break;

                case "SubjectPopEnglish":
                    time = clips[i].length;
                    break;

                case "CountdownAppear":
                    time = clips[i].length;
                    break;

                case "FinalPageShow":
                    time = clips[i].length;
                    break;

                case "QuizPageMove":
                    time = clips[i].length;
                    break;
            }
        }
        returnTime = time;
        return returnTime;
    }

    public void beginQuiz()
    {
        countdownDisplay.transform.parent.gameObject.SetActive(false);
        QuestionHandler.singleton.quizPage.SetActive(true);
        QuestionHandler.singleton.beginQuiz();
    }

    public void SetButtonFunction(string pageType)
    {
        mainButton.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        switch (pageType)
        {
            case "startGame":
                mainButton.transform.GetChild(1).GetComponent<Image>().sprite = buttonText[0];
                mainButton.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(700, 72);
                mainButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { startGame(); });
                break;

            case "mathStart":
                mainButton.transform.GetChild(1).GetComponent<Image>().sprite = buttonText[1];
                mainButton.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(410, 196);
                mainButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { shiftToCountDown(); });
                break;

            case "englishStart":
                mainButton.transform.GetChild(1).GetComponent<Image>().sprite = buttonText[2];
                mainButton.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(410, 196);
                mainButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { shiftToCountDown(); });
                break;

            case "clear":
                mainButton.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                break;
        }

        mainButton.SetActive(true);
    }
}
