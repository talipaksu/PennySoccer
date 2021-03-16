using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum States
    {
        IDLE, START, PLAYER_1_TURN, PLAYER_2_TURN, PLAYER_1_WIN, PLAYER_2_WIN, PLAYER_1_LOST, PLAYER_2_LOST
    }
    public Text scoreText;
    public Text timerText;
    private int matchTime = 120;
    private float startTime = 0;
    private bool matchActive = false;

    public GameObject[] playerPrefabs;
    private GameObject player1;
    private GameObject player2;

    public GameObject cameraAndCoinPrefab;
    public static GameObject cameraAndCoindReal;
    public static GameObject coinReal;
    private Player player1Status;
    private Player player2Status;
    private SoccerBall soccerBall;
    private TakeShot takeShot;
    private GameObject cameraController;
    private static Animator animator;
    public static States state = States.IDLE;

    public static bool turnFlag;

    void Awake()
    {
        cameraAndCoindReal = Instantiate(cameraAndCoinPrefab, new Vector3(0, 1.3f, 0), Quaternion.identity);
        coinReal = cameraAndCoindReal.transform.Find("Coin").gameObject;
        cameraController = cameraAndCoindReal.transform.Find("CameraController").gameObject;
        animator = cameraController.GetComponent<Animator>();
        soccerBall = coinReal.GetComponent<SoccerBall>();
    }

    void Start()
    {
        state = States.PLAYER_1_TURN;

        SetTriggers("Player1TurnTrigger");

        player1 = Instantiate(playerPrefabs[0], new Vector3(0, 0, 0), Quaternion.identity);
        player2 = Instantiate(playerPrefabs[1], new Vector3(0, 0, 0), Quaternion.identity);

        player1Status = player1.GetComponent<Player>();
        player2Status = player2.GetComponent<Player>();

        SetTimeDisplay(matchTime);
        startTime = Time.time;
        matchActive = true;

        soccerBall.player1GoalEvent.AddListener(IncrementPlayer1Score);
        soccerBall.player2GoalEvent.AddListener(IncrementPlayer2Score);

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
    private void IncrementPlayer1Score()
    {
        if (matchActive)
        {
            player1Status.score++;
            scoreText.text = "Score : " + player1Status.score + " : " + player2Status.score;

            /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player1Turn"))
            {
                animator.ResetTrigger("Player1TurnTrigger");
                SetTriggers("Player2TurnTrigger");
                state = States.PLAYER_2_TURN;
            }*/
        }
    }

    private void IncrementPlayer2Score()
    {
        if (matchActive)
        {
            player2Status.score++;
            scoreText.text = "Score : " + player1Status.score + " : " + player2Status.score;
            /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player2Turn"))
            {
                animator.ResetTrigger("Player2TurnTrigger");
                SetTriggers("Player1TurnTrigger");
                state = States.PLAYER_1_TURN;
            }*/
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


    public static void SetTriggerAndChangeStateForPlayer1()
    {
        animator.ResetTrigger("Player2TurnTrigger");
        SetTriggers("Player1TurnTrigger");
        state = States.PLAYER_1_TURN;

    }
    public static void SetTriggerAndChangeStateForPlayer2()
    {
        animator.ResetTrigger("Player1TurnTrigger");
        SetTriggers("Player2TurnTrigger");
        state = States.PLAYER_2_TURN;

    }
    public static void SetTriggers(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    public static void ChangeState(States stateTo)
    {
        if (state == stateTo)
            return;
        state = stateTo;
    }

    public static bool IsState(States stateTo)
    {
        if (state == stateTo)
            return true;
        return false;
    }



}
