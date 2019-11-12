using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubjectStartHandler : MonoBehaviour
{
    public static SubjectStartHandler singleton;
    public GameObject subjectStartPage;
    public Sprite mathStart, englishStart;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        subjectStartPage.SetActive(false);
    }

    public void mathStartDisplay()
    {
        subjectStartPage.GetComponent<Image>().sprite = mathStart;
        subjectStartPage.SetActive(true);
        BackendHandler.singleton.SetButtonFunction("mathStart");
    }

    public void englishStartDisplay()
    {
        subjectStartPage.GetComponent<Image>().sprite = englishStart;
        subjectStartPage.SetActive(true);
        BackendHandler.singleton.currSubject = BackendHandler.SUBJECTS.ENGLISH;
        LoadXMLFile.singleton.changeXML("english");
        BackendHandler.singleton.SetButtonFunction("englishStart");
    }
}
