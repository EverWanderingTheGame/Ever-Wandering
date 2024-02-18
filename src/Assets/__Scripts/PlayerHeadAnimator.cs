using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerHeadAnimator : MonoBehaviour
{
    private Animator animator;

    float horizontalMove = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!GameHUD.isPaused || LevelManager.instance.isLoading)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }
    }
}
