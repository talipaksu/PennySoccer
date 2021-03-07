using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    public GameController controller;
    private Vector3 mouseClickPoint;
    private Vector3 mouseReleasePoint;
    private Vector3 lineStartPoint;
    private Vector3 lineEndPoint;
    private Vector3 distanceVector;
    private Vector3 direction;
    private Vector3 tmpPoint;
    private float distance;
    public float powerFactor = 250.0f;
    private bool shotEnable;
    private bool shotFired;

    private LineRenderer line;
    private Rigidbody rigidbody;
    Camera camera;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {

        if (shotEnable)
        {
            tmpPoint.x = Input.mousePosition.x;
            tmpPoint.y = Input.mousePosition.y;
            tmpPoint.z = transform.position.z - Camera.main.transform.position.z;
            if (Input.GetMouseButtonDown(0))
            {
                mouseClickPoint = Camera.main.ScreenToWorldPoint(tmpPoint);
                lineStartPoint = this.gameObject.transform.position;
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
                shotEnable = false;

            }
        }
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shotEnable = true;
        }

    }

    private void DrawLine(Vector3 startPoint, Vector3 endPoint)
    {
        startPoint.y = 0;
        endPoint.y = 0;

        line.positionCount = 2;
        line.SetPosition(0, startPoint);
        line.SetPosition(1, endPoint);
    }

    private void EndLine()
    {
        line.positionCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {

        controller.IncrementScore(other.gameObject.tag);
        rigidbody.velocity = Vector3.zero;
        this.gameObject.transform.position = Vector3.up;
    }
}
