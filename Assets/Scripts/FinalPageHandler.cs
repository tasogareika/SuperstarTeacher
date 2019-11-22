using TMPro;
using System.IO;
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
    public TextMeshProUGUI gamesPlayed;
    public Image finalScoreDisplay, highScoreDisplay, mathScoreDisplay, englishScoreDisplay;
    public List<Sprite> scoreNumImg, highScoreImg;
    private int currHighScore, timesPlayed;
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
        //get data from txt file
        string[] data = File.ReadAllLines(BackendHandler.singleton.pathFile);
        string[] score = data[0].Split(':');
        currHighScore = int.Parse(score[1]);
        string[] played = data[1].Split(':');
        timesPlayed = int.Parse(played[1]);

        finalScoreDisplay.sprite = scoreNumImg[BackendHandler.totalScore];
        timesPlayed++;
        gamesPlayed.text = timesPlayed.ToString();
        
        if (currHighScore < BackendHandler.totalScore)
        {
            newHighScoreImg.SetActive(true);
            currHighScore = BackendHandler.totalScore;
            highScoreDisplay.sprite = highScoreImg[currHighScore];
        } else
        {
            newHighScoreImg.SetActive(false);
            highScoreDisplay.sprite = highScoreImg[currHighScore];
        }

        //save updated data
        File.Delete(BackendHandler.singleton.pathFile);
        StreamWriter writer = new StreamWriter(BackendHandler.singleton.pathFile, true);
        writer.WriteLine("High Score:" + currHighScore + "\nTimes Played:" + gamesPlayed.text);
        writer.Close();

        finalPage.SetActive(true);
        thisAnim.enabled = true;
        thisAnim.Play("FinalPageShow");
        BackendHandler.singleton.playSFX(6);
        StartCoroutine(finalAnim(BackendHandler.singleton.getAnimTime(thisAnim) + 0.2f));
    }

    public void backToStart()
    {
        BackendHandler.singleton.playSFX(1);
        finalPage.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void restartQuiz()
    {
        BackendHandler.singleton.playSFX(1);
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
