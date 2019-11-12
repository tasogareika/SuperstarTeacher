using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalPageHandler : MonoBehaviour
{
    public static FinalPageHandler singleton;
    public GameObject finalPage, newHighScoreImg;
    public TextMeshProUGUI mathScore, englishScore, highScore;
    public Image firstNumDisplay, secondNumDisplay;
    public List<Sprite> scoreNumImg;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        finalPage.SetActive(false);
    }

    public void finalPageShow()
    {
        if (BackendHandler.totalScore < 10)
        {
            firstNumDisplay.sprite = scoreNumImg[0];
            secondNumDisplay.sprite = scoreNumImg[BackendHandler.totalScore];
        } else
        {
            string s = BackendHandler.totalScore.ToString();
            firstNumDisplay.sprite = scoreNumImg[int.Parse(s[0].ToString())];
            secondNumDisplay.sprite = scoreNumImg[int.Parse(s[1].ToString())];
        }

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
    }

    public void backToStart()
    {
        finalPage.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void restartQuiz()
    {
        finalPage.SetActive(false);
        BackendHandler.totalScore = 0;
        BackendHandler.singleton.currSubject = BackendHandler.SUBJECTS.MATH;
        LoadXMLFile.singleton.changeXML("math");
        BackendHandler.singleton.startCountdown();
    }
}
