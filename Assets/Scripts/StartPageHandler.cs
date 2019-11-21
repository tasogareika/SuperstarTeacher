using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPageHandler : MonoBehaviour
{
    public static StartPageHandler singleton;
    [SerializeField] private GameObject startPage, videoPanel;
    private Animator thisAnimator, videoAnimator;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        thisAnimator = startPage.GetComponent<Animator>();
        videoAnimator = videoPanel.GetComponent<Animator>();
        startPage.SetActive(true);
        thisAnimator.Play("StartPageAppear");
        StartCoroutine(startTitlePulse(BackendHandler.singleton.getAnimTime(thisAnimator)));
        videoPanel.SetActive(false);
    }

    public void shiftToVideo()
    {
        videoPanel.SetActive(true);
        startPage.GetComponent<Animator>().enabled = true;
        thisAnimator.Play("StartPageMove");
        BackendHandler.singleton.mainButtonShow();
        videoAnimator.Play("VideoAppear");
        StartCoroutine(startVideo(BackendHandler.singleton.getAnimTime(thisAnimator) + 0.2f));
    }

    public void videoEnd()
    {
        videoAnimator.Play("VideoMove");
        BackendHandler.singleton.mainButtonReturn();
        SubjectStartHandler.singleton.mathStartDisplay();
        StartCoroutine(showMath(BackendHandler.singleton.getAnimTime(thisAnimator) + 0.3f));
    }

    public void restartGame()
    {
        BackendHandler.singleton.mainButton.SetActive(true);
        BackendHandler.singleton.mainButtonReturn();
        SubjectStartHandler.singleton.mathStartDisplay();
        StartCoroutine(showMath(BackendHandler.singleton.getAnimTime(thisAnimator) + 0.3f));
    }

    public void skipVideo()
    {
        StreamVideo.singleton.videoSkip();
    }

    private IEnumerator startTitlePulse(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        startPage.GetComponent<Animator>().enabled = false;
        BackendHandler.singleton.buttonAnimator.enabled = false;
        BackendHandler.singleton.SetButtonFunction("startGame");
        BackendHandler.singleton.playBGM("titleBGM");
    }

    private IEnumerator startVideo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        BackendHandler.singleton.stopBGM();
        StreamVideo.singleton.startVideo();
    }

    private IEnumerator showMath(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        videoPanel.SetActive(false);
        startPage.SetActive(false);
        BackendHandler.singleton.SetButtonFunction("mathStart");
    }
}
