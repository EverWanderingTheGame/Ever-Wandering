using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public TriggerEvent trigger;
    public Animator animator;
    [Range(1f, 100)]public float jumpForce = 10f;

    private CharacterController2D characterController;

    private void Awake()
    {
        characterController = FindObjectOfType<CharacterController2D>();
    }

    public void triggerEntered()
    {
        trigger.triggered = true;
        animator.SetTrigger("Jump");
        characterController.Move(0, false, false);
        characterController.addForce(new Vector2(0, jumpForce));
    }

    public void triggerExited()
    {
        trigger.triggered = false;
        animator.SetTrigger("Return");
    }
}
