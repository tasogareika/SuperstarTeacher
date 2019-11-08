using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

//ref: https://mirimad.com/unity-play-video-on-canvas/

public class StreamVideo : MonoBehaviour
{
    public static StreamVideo singleton;
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    private bool hasPlayed;

    private void Awake()
    {
        singleton = this;
    }

    public void startVideo()
    {
        StartCoroutine(PlayVideo());
    }

    private void Update()
    {
        if (videoPlayer.isPlaying)
        {
            hasPlayed = true;
        }

        if (!videoPlayer.isPlaying && hasPlayed)
        {
            hasPlayed = false;
            StartPageHandler.singleton.videoEnd();
        }
    }

    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
    }

    public void videoSkip()
    {
        videoPlayer.Stop();
        audioSource.Stop();
    }
}