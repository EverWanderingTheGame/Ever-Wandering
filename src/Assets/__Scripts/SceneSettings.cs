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
    [Tooltip("Only in BUILD")] public bool disableCursor = false;
    [Space, Header("Audio")]
    [SerializeField] public bool muteAudio = false;
    [Space]

    GameObject Player;
    GameHUD gameHUD;
    PlayerMovement playerMovement;
    PlayerHeadAnimator playerHeadAnimator;

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
            GameManager.instance.player.SetActive(!disablePlayer);

            Utils.TeleportPlayerToSceneSettings();
            TrailManager.updateAllObjects();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SceneSettings))]
public class SceneSettingsEditor : Editor
{
    private GUIStyle boldLabelStyle;
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
        if (boldLabelStyle == null)
        {
            boldLabelStyle = new GUIStyle(EditorStyles.label);
            boldLabelStyle.fontStyle = FontStyle.Bold;
        }
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

        GUILayout.Label("EDITOR BUTTONS", boldLabelStyle);

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
