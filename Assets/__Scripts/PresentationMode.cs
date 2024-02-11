using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PresentationMode : MonoBehaviour
{
    public GameObject[] posistions;

    GameObject Player;
    CinemachineVirtualCamera virtualCamera;
    CinemachineComponentBase componentBase;
    int posIndex = 0;
    int posIndexMax;

    void Awake()
    {
        Player = GameManager.instance.Player;

        // Set Cinemachine DeadZone to 0
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


        posIndexMax = posistions.Length - 1;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump"))
        {
            Debug.Log(posIndex);
            Player.transform.position = posistions[posIndex].transform.position;

            if(posIndexMax == posIndex) posIndex = 0;
            else posIndex++;
        }
    }
}
