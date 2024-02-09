using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneSettings : MonoBehaviour
{
    [Header("OnStart Scene Settings")]
    [SerializeField] private bool disableHUD = false;
    [SerializeField] private bool disablePlayer = false;
    [SerializeField] private bool disableCameraFX = false;
    [SerializeField] private bool muteAudio = false;

    public static GameObject HUD;
    public static GameObject Player;
    public static GameObject CameraFX;

    void Start()
    {
        GameManager.TeleportPlayerToDefaultPosistion();
        TrailManager.updateAllObjects();
        GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        FindObjectOfType<AudioManager>().Stop("JumpDrop");

        if (GameObject.Find("HUD") != null) HUD = GameObject.Find("HUD");
        if (disableHUD && HUD != null) HUD.SetActive(false);
        else if (!disableHUD && HUD != null) HUD.SetActive(true);

        if (GameObject.Find("Player") != null) Player = GameObject.Find("Player");
        if (disablePlayer && Player != null) Player.SetActive(false);
        else if (!disablePlayer && Player != null) Player.SetActive(true);

        if (GameObject.Find("Particles/FX") != null) CameraFX = GameObject.Find("Particles/FX");
        if (disableCameraFX && CameraFX != null) CameraFX.SetActive(false);
        else if (!disableCameraFX && CameraFX != null) CameraFX.SetActive(true);

        if (muteAudio) AudioManager.Mute();
        else if (!muteAudio) AudioManager.UnMute();
    }
}
