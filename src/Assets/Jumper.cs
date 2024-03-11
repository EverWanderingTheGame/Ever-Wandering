using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public TriggerEvent trigger;
    public Animator animator;
    [Range(1000, 2000)]public float jumpForce = 1100f;

    private CharacterController2D characterController;

    private void Awake()
    {
        characterController = FindObjectOfType<CharacterController2D>();
    }

    public void triggerEntered()
    {
        trigger.triggered = true;
        animator.SetTrigger("Jump");
        characterController.Move(0, false, true);
        characterController.addJumpForce(jumpForce);
    }

    public void triggerExited()
    {
        trigger.triggered = false;
        animator.SetTrigger("Return");
    }
}
