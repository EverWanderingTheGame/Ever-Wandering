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
    [Space, Header("Player")]
    [SerializeField] public bool disablePlayer = false;
    [SerializeField] public bool disablePlayerMovement = false;
    [Space, Header("Graphics")]
    [SerializeField] public bool disableHUD = false;
    [SerializeField] public bool disablePauseMenu = false;
    [SerializeField] public bool disableCameraFX = false;
    [SerializeField, Tooltip("Only in BUILD")] public bool disableCursor = false;
    [Space, Header("Audio")]
    [SerializeField] public bool muteAudio = false;

    public static GameObject Player;
    public static GameObject CameraFX;

    GameHUD gameHUD;
    PlayerMovement playerMovement;
    PlayerHeadAnimator playerHeadAnimator;

    void Awake()
    {
#if UNITY_EDITOR
        if (UnityEditor.BuildPipeline.isBuildingPlayer) return;
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
        }
#endif
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        if (BuildPipeline.isBuildingPlayer) return;
#endif

        gameHUD = FindObjectOfType<GameHUD>(true);
        playerMovement = FindObjectOfType<PlayerMovement>(true);
        playerHeadAnimator = FindObjectOfType<PlayerHeadAnimator>(true);

        applySceneSettings();
    }

    public void applySceneSettings()
    {
        gameHUD.PlayerHUD.SetActive(!disableHUD);
        gameHUD.unpauseable = disablePauseMenu;
        gameHUD.CameraFX.SetActive(!disableCameraFX);
#if !UNITY_EDITOR
        Cursor.visible = !disableCursor;
#endif

        playerMovement.enabled = !disablePlayerMovement;
        playerHeadAnimator.enabled = !disablePlayerMovement;

        if (muteAudio) AudioManager.Mute();
        else AudioManager.UnMute();
    }

    void Start()
    {
        if (Application.isPlaying)
        {
            if (GameObject.Find("Player") != null) Player = GameObject.Find("Player");
            if (disablePlayer && Player != null) Player.SetActive(false);
            else if (!disablePlayer && Player != null) Player.SetActive(true);

            GameManager.TeleportPlayerToDefaultPosistion();
            TrailManager.updateAllObjects();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SceneSettings))]
public class SceneSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SceneSettings sceneSettings = (SceneSettings)target;

        GUI.backgroundColor = new Color(202f, 74f, 74f, 128f) / 255f * new Vector4(5, 5, 5, 1);
        if (GUILayout.Button("Apply Scene Settings (IN EDIT MODE)"))
        {
            sceneSettings.applySceneSettings();
        }
    }
}
#endif
