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

    void Start()
    {
    }

    void Update()
    {
    }
    void OnMouseOver()
    {
        //Debug.Log("soccerball onmouseover");
        if (Input.GetMouseButtonDown(0))
        {
            onMouseOverEvent.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LeftGoalTrigger")
        {
            player1GoalEvent.Invoke();
        }
        else if (other.gameObject.tag == "RightGoalTrigger")
        {
            player2GoalEvent.Invoke();
        }

        rigidbody.velocity = Vector3.zero;
        this.gameObject.transform.position = Vector3.up;
    }
}
