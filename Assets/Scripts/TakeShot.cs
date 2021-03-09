using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool fingerOverCoin;
    private GameObject coinReal;
    private SoccerBall soccerBall;
    private Rigidbody rigidbody;
    Camera camera;

    void Start()
    {
        coinReal = GameController.coinReal;
        rigidbody = coinReal.GetComponent<Rigidbody>();
        soccerBall = coinReal.GetComponent<SoccerBall>();
        soccerBall.onMouseOverEvent.AddListener(MouseOverOnCoin);
        camera = Camera.main;
    }

    void Update()
    {
        if (fingerOverCoin)
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


                distanceVector.x = mouseClickPoint.x - mouseReleasePoint.x;
                distanceVector.y = mouseClickPoint.y - mouseReleasePoint.y;
                distanceVector.z = mouseClickPoint.z - mouseReleasePoint.z;

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
                fingerOverCoin = false;
            }
        }
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
        fingerOverCoin = true;
    }

}
