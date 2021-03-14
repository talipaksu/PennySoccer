using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoccerBall : MonoBehaviour
{
    public UnityEvent player1GoalEvent;
    public UnityEvent player2GoalEvent;
    public UnityEvent onMouseOverEvent;
    public Rigidbody rigidbody;

    private static bool isMoving;
    public static bool IsMoving
    {
        get { return isMoving; }
        set
        {
            isMoving = value;
        }
    }

    void Awake()
    {

    }
    void Start()
    {

    }

    void Update()
    {

        //Debug.Log("soccerball is moving : " + isMoving);
    }
    void OnMouseOver()
    {
        //Debug.Log("soccerball onmouseover");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("SoccerBall - OnMouseOver - GetMouseButtonDown");
            onMouseOverEvent.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1GoalTrigger")
        {
            player2GoalEvent.Invoke();
        }
        else if (other.gameObject.tag == "Player2GoalTrigger")
        {
            player1GoalEvent.Invoke();
        }

        rigidbody.velocity = Vector3.zero;
        this.gameObject.transform.position = Vector3.up;
    }
}
