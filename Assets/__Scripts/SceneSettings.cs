using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteInEditMode]
[DefaultExecutionOrder(-19998)]
public class SceneSettings : MonoBehaviour
{
    [Header("OnStart Scene Settings")]
    [SerializeField] public bool disableHUD = false;
    [SerializeField] public bool disablePlayer = false;
    [SerializeField] public bool disableCameraFX = false;
    [SerializeField] public bool muteAudio = false;

    public static GameObject HUD;
    public static GameObject Player;
    public static GameObject CameraFX;


    void Awake()
    {
#if UNITY_EDITOR
        if (UnityEditor.BuildPipeline.isBuildingPlayer) return; // CORRUPTS THE SCENES!
        if (GameObject.Find("Game Manager") == null)
        {
            if (UnityEditor.EditorApplication.isPlaying)
            {
                SceneManager.LoadScene("Bootstrap", LoadSceneMode.Additive);
            }
            else
            {
                var scene = EditorSceneManager.OpenScene("Assets/__Scenes/Bootstrap.unity", OpenSceneMode.Additive);
                EditorSceneManager.SetActiveScene(scene);
                EditorSceneManager.MoveSceneBefore(scene, EditorSceneManager.GetSceneAt(0));
            }
            Shader.SetGlobalFloat("_TessMult", 1f);
        }
#endif
    }

    private void OnEnable()
    {
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
        else AudioManager.UnMute();
    }

    void Start()
    {
        if (Application.isPlaying)
        {
            GameManager.TeleportPlayerToDefaultPosistion();
            TrailManager.updateAllObjects();
        }
    }
}
