using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TakeShot : MonoBehaviour
{
    public LineRenderer line;
    private Vector3 mouseClickPoint;
    private Vector3 mouseReleasePoint;
    private Vector3 lineStartPoint;
    private Vector3 lineEndPoint;
    private Vector3 distanceVector;
    private Vector3 direction;
    private Vector3 tmpPoint;
    private float distance;
    public float powerFactor = 250.0f;
    private GameObject coinReal;
    private SoccerBall soccerBall;
    private Rigidbody rigidbody;
    Camera camera;

    public UnityEvent player1TurnEvent;
    public UnityEvent player2TurnEvent;
    private bool shotFired;
    private static bool mouseClicked;

    Cinemachine.CinemachineImpulseSource impulseSource;
    void Start()
    {
        coinReal = GameController.coinReal;
        rigidbody = coinReal.GetComponent<Rigidbody>();
        soccerBall = coinReal.GetComponent<SoccerBall>();
        soccerBall.onMouseOverEvent.AddListener(MouseOverOnCoin);
        impulseSource = coinReal.GetComponent<Cinemachine.CinemachineImpulseSource>();

        camera = Camera.main;
    }

    void Update()
    {

    }

    public IEnumerator TakeAShot()
    {
        mouseClicked = true;
        while (!shotFired)
        {
            tmpPoint.x = Input.mousePosition.x;
            tmpPoint.y = Input.mousePosition.y;
            tmpPoint.z = GameController.coinReal.transform.position.z - Camera.main.transform.position.z;
            if (Input.GetMouseButtonDown(0))
            {
                mouseClickPoint = Camera.main.ScreenToWorldPoint(tmpPoint);
                lineStartPoint = GameController.coinReal.transform.position;
            }
            if (Input.GetMouseButton(0))
            {
                mouseReleasePoint = Camera.main.ScreenToWorldPoint(tmpPoint);

                if (GameController.IsState(GameController.States.PLAYER_1_TURN))
                {
                    distanceVector.x = -1 * (mouseClickPoint.x - mouseReleasePoint.x);
                    distanceVector.y = -1 * (mouseClickPoint.y - mouseReleasePoint.y);
                    distanceVector.z = -1 * (mouseClickPoint.z - mouseReleasePoint.z);
                }
                else
                {
                    distanceVector.x = mouseClickPoint.x - mouseReleasePoint.x;
                    distanceVector.y = mouseClickPoint.y - mouseReleasePoint.y;
                    distanceVector.z = mouseClickPoint.z - mouseReleasePoint.z;
                }

                lineEndPoint = lineStartPoint - distanceVector;

                DrawLine(lineStartPoint, lineEndPoint);
            }
            if (Input.GetMouseButtonUp(0))
            {

                distance = Vector3.Distance(lineStartPoint, lineEndPoint);
                direction = lineStartPoint - lineEndPoint;
                direction.y = 0;
                direction = direction.normalized;
                rigidbody.AddForce(direction * powerFactor * distance);
                EndLine();
                shotFired = true;
                impulseSource.GenerateImpulse(camera.transform.forward * distance/10);
            }
            Debug.Log("TakeAShot WHILE İÇİ");
            yield return null;
        }
        Debug.Log("TakeAShot WHILE SONRASI");
        yield return StartCoroutine(CheckMoving());
        if (SoccerBall.IsMoving == false)
        {
            ChangeTurn();
        }
    }
    public IEnumerator CheckMoving()
    {
        Vector3 startPos = transform.position;
        yield return new WaitForSeconds(3f);
        Vector3 finalPos = transform.position;
        if (startPos.x != finalPos.x || startPos.y != finalPos.y)
        {
            SoccerBall.IsMoving = true;
        }
        else
        {
            SoccerBall.IsMoving = false;
        }

    }
    public void ChangeTurn()
    {
        if (GameController.state == GameController.States.PLAYER_1_TURN)
        {
            GameController.SetTriggerAndChangeStateForPlayer2();
            Debug.Log("GameController.SetTriggerAndChangeStateForPlayer2();");
        }
        else if (GameController.state == GameController.States.PLAYER_2_TURN)
        {
            GameController.SetTriggerAndChangeStateForPlayer1();
            Debug.Log("GameController.SetTriggerAndChangeStateForPlayer1();");
        }
        shotFired = false;
        SoccerBall.IsMoving = true;
        mouseClicked = false;
    }
    public void DrawLine(Vector3 startPoint, Vector3 endPoint)
    {
        startPoint.y = 0;
        endPoint.y = 0;

        line.positionCount = 2;
        line.SetPosition(0, startPoint);
        line.SetPosition(1, endPoint);
    }

    public void EndLine()
    {
        line.positionCount = 0;
    }

    private void MouseOverOnCoin()
    {
        if (mouseClicked == false)
        {
            Debug.Log("TakeAShot - MouseOverCoin");
            StartCoroutine(TakeAShot());
        }
    }

}
