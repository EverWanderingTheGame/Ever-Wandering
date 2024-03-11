using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public SpriteRenderer Head;
    [Range(0, 200)][SerializeField] public float runSpeed = 40f;
    public Animator BodyAnimator;
    public Animator HeadAnimator;

    [HideInInspector] public bool disablePlayerMovement;

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D playerRigidBody;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    float zoomLevel = 0f;
    CinemachineVirtualCamera _virtualCamera;
    float _orthoSize;
    float orthoSize;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerRigidBody = GetComponent<Rigidbody2D>();

        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _orthoSize = _virtualCamera.m_Lens.OrthographicSize;
        orthoSize = _orthoSize;
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed * (disablePlayerMovement ? 0 : 1);
        
        if ((GameManager.instance.gameState == GameState.Playing || GameManager.instance.gameState == GameState.Prsentation) && !disablePlayerMovement && !LevelManager.instance.isLoading)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }

            // Zoom in/out with Z
            zoomLevel = (_orthoSize * 70) / 100;
            if (Input.GetKey(KeyCode.Z)) orthoSize = Mathf.Lerp(orthoSize, zoomLevel, 10 * Time.deltaTime);   
            else orthoSize = Mathf.Lerp(orthoSize, _orthoSize, 10 * Time.deltaTime);
            orthoSize = Mathf.Round(orthoSize * 100) / 100;
            _virtualCamera.m_Lens.OrthographicSize = orthoSize;
        } else
        {
            if (Input.GetButtonDown("Jump"))
            {
                jump = false;
            }
        }

        BodyAnimator.SetFloat("Speed", Mathf.Abs(playerRigidBody.velocity.x));
        HeadAnimator.SetFloat("Speed", Mathf.Abs(playerRigidBody.velocity.x));
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
