﻿using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TargetHandler : MonoBehaviour
{
    public TextMeshProUGUI answerDisplay;
    public bool answer;
    public Sprite normalBtn, correctImg, wrongImg;
    public Sprite circleNorm, circleCorrect, circleWrong;
    public Sprite recNorm, recCorrect, recWrong;
    private Color normalColor, shiftColor;

    private void Start()
    {
        normalColor = new Color(0.2980392f, 0.2980392f, 0.2980392f, 1);
        shiftColor = new Color(1, 1, 1, 1);
    }

    public void checkAnswer()
    {
        QuestionHandler.singleton.timerRun = false;
        if (answer)
        {
            GetComponent<Image>().sprite = correctImg;
            QuestionHandler.singleton.correctResponse(this.gameObject);
            BackendHandler.singleton.playSFX(4);
            StartCoroutine(nextQuestion(1f));
        }
        else
        {
            GetComponent<Image>().sprite = wrongImg;
            QuestionHandler.singleton.showCorrectAnswer();
            BackendHandler.singleton.playSFX(5);
            StartCoroutine(nextQuestion(2f));
        }
        answerDisplay.color = shiftColor;
        QuestionHandler.singleton.closeButtons();
    }

    public void displayAnswer()
    {
        GetComponent<Image>().sprite = correctImg;
        answerDisplay.color = shiftColor;
        StartCoroutine(imageBack(2f));
    }

    public void switchImages(string shape)
    {
        switch (shape)
        {
            case "circle":
                normalBtn = circleNorm;
                correctImg = circleCorrect;
                wrongImg = circleWrong;
                GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);
                break;

            case "rectangle":
                normalBtn = recNorm;
                correctImg = recCorrect;
                wrongImg = recWrong;
                GetComponent<RectTransform>().sizeDelta = new Vector2(420, 270);
                break;
        }

        GetComponent<Image>().sprite = normalBtn;
    }

    private IEnumerator imageBack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<Image>().sprite = normalBtn;
        answerDisplay.color = normalColor;
    }

    private IEnumerator nextQuestion(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<Image>().sprite = normalBtn;
        answerDisplay.color = normalColor;
        QuestionHandler.singleton.answerToggle(answer);
    }
}