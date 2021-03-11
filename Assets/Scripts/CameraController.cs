using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraTrigger
{
    Player1Turn,
    Player2Turn
}
public class CameraController : MonoBehaviour
{
    private Animator animator;
    public Animator Animator
    {
        get
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            return animator;
        }
    }

    public void TriggerCamera(CameraTrigger trigger)
    {
        Animator.SetTrigger(trigger.ToString());
    }
}
