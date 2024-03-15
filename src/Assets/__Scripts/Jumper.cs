using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public TriggerEvent trigger;
    public Animator animator;
    [Range(1f, 100)] public float jumpForce = 10f;
    public float cooldown = 1f;

    private float lastJump = 0f;

    public void triggerEntered()
    {
        if (Time.time - lastJump < cooldown) { return; }

        trigger.triggered = true;
        animator.SetTrigger("Jump");

        var rigidbody = trigger.TriggeredBy.gameObject.GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);

        lastJump = Time.time;
    }

    public void triggerExited()
    {
        trigger.triggered = false;
        animator.SetTrigger("Return");
    }
}
