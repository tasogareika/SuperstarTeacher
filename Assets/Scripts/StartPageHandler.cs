using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPageHandler : MonoBehaviour
{
    public static StartPageHandler singleton;
    [SerializeField] private GameObject startPage, videoPanel;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        videoPanel.SetActive(false);
    }

    public void playVideo()
    {
        videoPanel.SetActive(true);
        StreamVideo.singleton.startVideo();
    }

    public void videoEnd()
    {
        videoPanel.SetActive(false);
        startPage.SetActive(false);
        SubjectStartHandler.singleton.mathStartDisplay();
    }
}
