using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TargetHandler : MonoBehaviour
{
    public TextMeshProUGUI answerDisplay;
    public bool answer;
    public Sprite normalBtn, correctImg, wrongImg;

    public void checkAnswer()
    {
        QuestionHandler.singleton.timerRun = false;
        if (answer)
        {
            //BackendHandler.singleton.playCorrectAns();
            StartCoroutine(nextQuestion(0.5f));
        }
        else
        {
           // QuestionHandler.singleton.showCorrectAnswer();
           // BackendHandler.singleton.playWrongAns();
            StartCoroutine(nextQuestion(2f));
        }
        //QuestionHandler.singleton.closeButtons();
    }

    public void resetImage()
    {
        StartCoroutine(imageBack(2f));
    }

    private IEnumerator imageBack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<Image>().sprite = normalBtn;
    }

    private IEnumerator nextQuestion(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<Image>().sprite = normalBtn;
        QuestionHandler.singleton.answerToggle(answer);
    }
}