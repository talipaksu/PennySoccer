using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text scoreText;
    public Text timerText;
    public int matchTime = 120;
    private float startTime = 0;
    private bool matchActive = false;
    public GameObject prefab;
    private GameObject[] playerGameObjects = new GameObject[2];
    private Player[] players = new Player[2];


    void Start()
    {

        for (int i = 0; i < 2; i++)
        {
            playerGameObjects[i] = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            players[i] = playerGameObjects[i].GetComponent<Player>();
            players[i].score = 0;
        }

        SetTimeDisplay(matchTime);
        startTime = Time.time;
        matchActive = true;
    }

    public void IncrementScore(string tag)
    {
        if (matchActive)
        {
            if ("RightGoalTrigger" == tag)
            {
                players[0].score++;
            }
            else if ("LeftGoalTrigger" == tag)
            {
                players[1].score++;
            }

            scoreText.text = "Score : " + players[0].score + " : " + players[1].score;
        }

    }

    void Update()
    {
        if (Time.time - startTime < matchTime)
        {
            float ElapsedTime = Time.time - startTime;
            SetTimeDisplay(matchTime - ElapsedTime);
        }
        else
        {
            matchActive = false;
            SetTimeDisplay(0);
            scoreText.color = Color.red;
            timerText.color = Color.red;
        }
    }


    private void SetTimeDisplay(float timeDisplay)
    {
        timerText.text = "Time: " + GetTimeToDisplay(timeDisplay);
    }
    private string GetTimeToDisplay(float timeToShow)
    {
        int secondsToShow = Mathf.CeilToInt(timeToShow);
        int seconds = secondsToShow % 60;
        string secondsDisplay = (seconds < 10) ? "0" + seconds.ToString() : seconds.ToString();
        int minutes = (secondsToShow - seconds) / 60;
        return minutes.ToString() + ":" + secondsDisplay;
    }
}
