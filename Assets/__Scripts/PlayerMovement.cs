using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public SpriteRenderer Head;
    [Range(0, 200)][SerializeField] public float runSpeed = 40f;

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb;
    private Animator animator;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();   
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!GameHUD.isPaused)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
        }

    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
