using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[DisallowMultipleComponent]
[ExecuteInEditMode]
[DefaultExecutionOrder(-19998)]
public class SceneSettings : MonoBehaviour
{
    [Header("OnStart Scene Settings")]
    [SerializeField] public SceneReference nextScene;
    [Space, Header("Player")]
    [SerializeField] public bool disablePlayer = false;
    [SerializeField] public bool disablePlayerMovement = false;
    [SerializeField, Range(-1, 100), Tooltip("-1 = DO NOT CHANGE")] public int StartingLightPower = 100;
    [Space, Header("Graphics")]
    [SerializeField] public bool disableHUD = false;
    [SerializeField] public bool disablePauseMenu = false;
    [SerializeField] public bool disableCameraFX = false;
    [Tooltip("Only in BUILD")] public bool disableCursor = false;
    [Space, Header("Camera")]
    [SerializeField] public Transform cameraTarget;
    [SerializeField] public Vector2 cameraDeadZone = new Vector2(.2f, .1f);
    [SerializeField, Range(2, 20)] public float lensOrthoSize = 7;
    [Space, Header("Audio")]
    [SerializeField] public bool muteAudio = false;
    [Space]
    [Header("In EditMode Only")]
    [SerializeField] public bool disableLoadingScreen = true;
    [SerializeField] public bool disableHUDInEditMode = true;

    GameHUD gameHUD;
    PlayerMovement playerMovement;
    CinemachineVirtualCamera c_VirtualCam;
    CinemachineComponentBase componentBase;


    void Awake()
    {
#if UNITY_EDITOR // Load Bootstrap scene if not in play mode
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
        c_VirtualCam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        if (BuildPipeline.isBuildingPlayer) return;
#endif

        gameHUD = FindObjectOfType<GameHUD>(true);
        playerMovement = FindObjectOfType<PlayerMovement>(true);

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

        if (StartingLightPower != -1) TrailManager.LightPower = StartingLightPower;

        if (!Application.isPlaying) FindObjectOfType<LevelManager>()._loaderCanvas.SetActive(!disableLoadingScreen);
        if (disableHUDInEditMode && !Application.isPlaying) gameHUD.PlayerHUD.SetActive(false);

        if (muteAudio) AudioManager.Mute();
        else AudioManager.UnMute();

        c_VirtualCam.m_Lens.OrthographicSize = lensOrthoSize;

        if (Application.isPlaying) // If in GAME
        {
            playerMovement.disablePlayerMovement = disablePlayerMovement;
            if (cameraTarget != null)
            {
                c_VirtualCam.m_LookAt = cameraTarget;
                c_VirtualCam.m_Follow = cameraTarget;
            }
            else
            {
                c_VirtualCam.m_LookAt = GameManager.instance.player.transform;
                c_VirtualCam.m_Follow = GameManager.instance.player.transform;
            }
            if (cameraDeadZone != Vector2.zero)
            {
                componentBase = c_VirtualCam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineComponentBase;
                if (componentBase is CinemachineFramingTransposer)
                {
                    var framingTransposer = componentBase as CinemachineFramingTransposer;
                    framingTransposer.m_SoftZoneWidth = cameraDeadZone.x;
                    framingTransposer.m_SoftZoneHeight = cameraDeadZone.y;
                }
            }
        }
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            GameManager.instance.player.SetActive(!disablePlayer);
            TrailManager.updateAllObjects();
            Utils.TeleportPlayerToSceneSettings();
        }
    }
}

#if UNITY_EDITOR // Custom Editor
[CustomEditor(typeof(SceneSettings))]
public class SceneSettingsEditor : Editor
{
    private GUIStyle buttonStyle;

    bool disablePlayer;
    bool disablePlayerMovement;
    bool disableHUD;
    bool disablePauseMenu;
    bool disableCameraFX;
    bool disableCursor;
    bool muteAudio;

    private void InitializeStyles()
    {
        if (buttonStyle == null)
        {
            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.margin = new RectOffset(1, 1, 10, 10);
            buttonStyle.border = new RectOffset(0, 0, 0, 0);
            buttonStyle.padding = new RectOffset(0, 0, 5, 5);
        }
    }

    public void saveSetting()
    {
        SceneSettings sceneSettings = (SceneSettings)target;

        disablePlayer = sceneSettings.disablePlayer;
        disablePlayerMovement = sceneSettings.disablePlayerMovement;
        disableHUD = sceneSettings.disableHUD;
        disablePauseMenu = sceneSettings.disablePauseMenu;
        disableCameraFX = sceneSettings.disableCameraFX;
        disableCursor = sceneSettings.disableCursor;
        muteAudio = sceneSettings.muteAudio;
    }

    public void allBools(bool state)
    {
        SceneSettings sceneSettings = (SceneSettings)target;

        sceneSettings.disablePlayer = state;
        sceneSettings.disablePlayerMovement = state;
        sceneSettings.disableHUD = state;
        sceneSettings.disablePauseMenu = state;
        sceneSettings.disableCameraFX = state;
        sceneSettings.disableCursor = state;
        sceneSettings.muteAudio = state;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InitializeStyles();

        SceneSettings sceneSettings = (SceneSettings)target;

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = new Color(110f, 200f, 110f, 64f) / 255f * new Vector4(5, 5, 5, 1);
        if (GUILayout.Button("All", buttonStyle))
        {
            saveSetting();
            allBools(true);
            sceneSettings.applySceneSettings();
        }
        GUI.backgroundColor = new Color(87f, 35f, 30f, 64f) / 255f * new Vector4(5, 5, 5, 1);
        if (GUILayout.Button("None", buttonStyle))
        {
            saveSetting();
            allBools(false);
            sceneSettings.applySceneSettings();
        }
        GUI.backgroundColor = new Color(108f, 181f, 255f, 64f) / 255f * new Vector4(5, 5, 5, 1);
        if (GUILayout.Button("Revert Settings", buttonStyle))
        {
            sceneSettings.disablePlayer = disablePlayer;
            sceneSettings.disablePlayerMovement = disablePlayerMovement;
            sceneSettings.disableHUD = disableHUD;
            sceneSettings.disablePauseMenu = disablePauseMenu;
            sceneSettings.disableCameraFX = disableCameraFX;
            sceneSettings.disableCursor = disableCursor;
            sceneSettings.muteAudio = muteAudio;

            sceneSettings.applySceneSettings();
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("EDITOR BUTTONS", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = new Color(202f, 74f, 74f, 128f) / 255f * new Vector4(5, 5, 5, 1);
        if (GUILayout.Button("Apply Scene Settings", buttonStyle, GUILayout.Height(25)))
        {
            sceneSettings.applySceneSettings();
        }
        GUI.backgroundColor = new Color(35f, 82f, 171f, 128f) / 255f * new Vector4(5, 5, 5, 1);
        if (GUILayout.Button("Teleport Player To Scene Settings", buttonStyle, GUILayout.Height(25)))
        {
            Utils.TeleportPlayerToSceneSettings();
        }
        GUILayout.EndHorizontal();

    }
}
#endif
