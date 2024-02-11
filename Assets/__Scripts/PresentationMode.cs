using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PresentationMode : MonoBehaviour
{
    public Animator animator;
    public GameObject[] posistions;

    GameObject Player;
    CinemachineVirtualCamera virtualCamera;
    CinemachineComponentBase componentBase;
    int posIndex = 0;
    int posIndexMax;

    void Awake()
    {
        Player = GameManager.instance.player;

        // Cinemachine DeadZone and SoftZone
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineComponentBase;
        if (componentBase is CinemachineFramingTransposer)
        {
            var framingTransposer = componentBase as CinemachineFramingTransposer;
            framingTransposer.m_DeadZoneWidth = 0;
            framingTransposer.m_DeadZoneHeight = 0;
            framingTransposer.m_SoftZoneHeight = 2;
            framingTransposer.m_SoftZoneWidth = 2;
        }

        Player.transform.position = posistions[posIndex].transform.position;

        posIndexMax = posistions.Length;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            posIndex++;
            if (posIndexMax == posIndex) posIndex = 0;

            animator.SetInteger("Scene", posIndex);

            Player.transform.position = posistions[posIndex].transform.position;
        } else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            posIndex--;
            if (0 > posIndex) posIndex = 0;

            animator.SetInteger("Scene", posIndex);

            Player.transform.position = posistions[posIndex].transform.position;
        }
    }
}
