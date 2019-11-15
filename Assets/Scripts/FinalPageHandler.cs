using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalPageHandler : MonoBehaviour
{
    public static FinalPageHandler singleton;
    [SerializeField] private GameObject header, stars;
    public GameObject finalPage, newHighScoreImg;
    public TextMeshProUGUI mathScore, englishScore, highScore;
    public Image finalScoreDisplay;
    public List<Sprite> scoreNumImg;
    private Animator thisAnim;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        thisAnim = finalPage.GetComponent<Animator>();
        finalPage.SetActive(false);
    }

    public void finalPageShow()
    {
        finalScoreDisplay.sprite = scoreNumImg[BackendHandler.totalScore];

        if (!PlayerPrefs.HasKey("HighScore"))
        {
            newHighScoreImg.SetActive(true);
            highScore.text = BackendHandler.totalScore.ToString();
            PlayerPrefs.SetInt("HighScore", BackendHandler.totalScore);
            PlayerPrefs.Save();
        } else
        {
            int currHighScore = PlayerPrefs.GetInt("HighScore");
            if (currHighScore < BackendHandler.totalScore)
            {
                newHighScoreImg.SetActive(true);
                highScore.text = BackendHandler.totalScore.ToString();
                PlayerPrefs.SetInt("HighScore", BackendHandler.totalScore);
                PlayerPrefs.Save();
            } else
            {
                newHighScoreImg.SetActive(false);
                highScore.text = currHighScore.ToString();
            }
        }

        finalPage.SetActive(true);
        thisAnim.enabled = true;
        thisAnim.Play("FinalPageShow");
        StartCoroutine(finalAnim(BackendHandler.singleton.getAnimTime(thisAnim) + 0.2f));
    }

    public void backToStart()
    {
        finalPage.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void restartQuiz()
    {
        Destroy(header.GetComponent<ObjectPulse>());
        Destroy(stars.GetComponent<ObjectPulse>());
        finalPage.SetActive(false);
        BackendHandler.totalScore = 0;
        BackendHandler.singleton.currSubject = BackendHandler.SUBJECTS.MATH;
        LoadXMLFile.singleton.changeXML("math");
        StartPageHandler.singleton.restartGame();
    }

    private IEnumerator finalAnim (float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        thisAnim.enabled = false;
        header.AddComponent<ObjectPulse>();
        header.GetComponent<ObjectPulse>().approachSpeed = 0.003f;
        header.GetComponent<ObjectPulse>().growthBound = 1;
        header.GetComponent<ObjectPulse>().shrinkBound = 0.9f;

        stars.AddComponent<ObjectPulse>();
        stars.GetComponent<ObjectPulse>().approachSpeed = 0.003f;
        stars.GetComponent<ObjectPulse>().growthBound = 1;
        stars.GetComponent<ObjectPulse>().shrinkBound = 0.9f;
    }
}
