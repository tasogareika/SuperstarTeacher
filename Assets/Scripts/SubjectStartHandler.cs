﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubjectStartHandler : MonoBehaviour
{
    public static SubjectStartHandler singleton;
    public GameObject subjectStartPage, mathStart, englishStart;
    private Animator thisAnim;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        thisAnim = subjectStartPage.GetComponent<Animator>();
        subjectStartPage.SetActive(false);
    }

    public void mathStartDisplay()
    {
        subjectStartPage.SetActive(true);
        subjectStartPage.transform.GetChild(0).gameObject.SetActive(false);
        subjectStartPage.transform.GetChild(1).gameObject.SetActive(false);
        thisAnim.Play("SubjectAppear");
        StartCoroutine(startMathAnim(BackendHandler.singleton.getAnimTime(thisAnim) + 0.2f));
    }

    public void englishStartDisplay()
    {
        englishStart.SetActive(true);
        mathStart.SetActive(false);
        subjectStartPage.SetActive(true);
        BackendHandler.singleton.currSubject = BackendHandler.SUBJECTS.ENGLISH;
        LoadXMLFile.singleton.changeXML("english");
        BackendHandler.singleton.SetButtonFunction("englishStart");
    }

    public void moveToCountdown()
    {
        thisAnim.Play("SubjectMove");
    }

    private IEnumerator startMathAnim (float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        subjectStartPage.transform.GetChild(0).gameObject.SetActive(true);
        subjectStartPage.transform.GetChild(1).gameObject.SetActive(true);
        englishStart.SetActive(false);
        mathStart.SetActive(true);
        thisAnim.Play("SubjectPopMath");
        StartCoroutine(doAnimations(BackendHandler.singleton.getAnimTime(thisAnim) + 0.1f));
    }

    private IEnumerator doAnimations (float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        thisAnim.Play("SubjectIdle");
    }
}
