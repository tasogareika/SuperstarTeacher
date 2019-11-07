using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSpawners : MonoBehaviour
{
    public static TargetSpawners singleton;
    [SerializeField] private GameObject quizPage;
    public List<GameObject> targetList;
    public GameObject spawnPoint;
    private float targetWidth, targetHeight;
    private Vector2 lastSpawnPos;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        lastSpawnPos = spawnPoint.GetComponent<RectTransform>().anchoredPosition;
        quizPage.SetActive(false);
    }

    public void spawnTargets()
    {
        for (int i = 0; i < targetList.Count; i++)
        {
            GameObject target = targetList[i];

            targetWidth = target.GetComponent<RectTransform>().rect.width;
            targetHeight = target.GetComponent<RectTransform>().rect.height;
            target.transform.SetParent(quizPage.transform);
            target.transform.localScale = Vector3.one;
            float maxX = Screen.width - targetWidth;
            float minY = Screen.height * -0.55f;
            float maxY = (Screen.height - targetHeight) * -1;
            float xVariance = Mathf.RoundToInt(Random.Range(targetWidth * 0.5f, Screen.width * 0.3f));
            float yVariance = Mathf.RoundToInt(Random.Range(targetHeight * 0.5f, Screen.height * 0.2f));

            switch (i)
            {
                case 0:
                    target.GetComponent<RectTransform>().anchoredPosition = new Vector2(lastSpawnPos.x + xVariance, lastSpawnPos.y + yVariance);
                    break;

                case 1:
                    target.GetComponent<RectTransform>().anchoredPosition = new Vector2(lastSpawnPos.x - xVariance, lastSpawnPos.y - yVariance);
                    break;

                case 2:
                    target.GetComponent<RectTransform>().anchoredPosition = new Vector2(lastSpawnPos.x + xVariance, lastSpawnPos.y - yVariance);
                    break;

                case 3:
                    target.GetComponent<RectTransform>().anchoredPosition = new Vector2(lastSpawnPos.x - xVariance, lastSpawnPos.y + yVariance);
                    break;
            }

            //check to see if UI element has crossed limit and shift them if so
            if (target.GetComponent<RectTransform>().anchoredPosition.x > maxX)
            {
                target.GetComponent<RectTransform>().anchoredPosition = new Vector2(maxX, target.GetComponent<RectTransform>().anchoredPosition.y);
            }

            if (target.GetComponent<RectTransform>().anchoredPosition.y < maxY)
            {
                target.GetComponent<RectTransform>().anchoredPosition = new Vector2(target.GetComponent<RectTransform>().anchoredPosition.x, maxY);
            }

            if (target.GetComponent<RectTransform>().anchoredPosition.y > minY)
            {
                target.GetComponent<RectTransform>().anchoredPosition = new Vector2(target.GetComponent<RectTransform>().anchoredPosition.x, minY);
            }
        }
    }
}
