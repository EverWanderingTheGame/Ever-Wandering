using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneReference : ISerializationCallbackReceiver
{
#if UNITY_EDITOR
    [SerializeField] public Object sceneAsset = null;
    bool IsValidSceneAsset
    {
        get
        {
            if (sceneAsset == null)
                return false;
            return sceneAsset.GetType().Equals(typeof(SceneAsset));
        }
    }
#endif
    [SerializeField]
    private string scenePath = string.Empty;

    public string ScenePath
    {
        get
        {
#if UNITY_EDITOR
            return GetScenePathFromAsset();
#else
            return scenePath;
#endif
        }
        set
        {
            scenePath = value;
#if UNITY_EDITOR
            sceneAsset = GetSceneAssetFromPath();
#endif
        }
    }

    public static implicit operator string(SceneReference sceneReference)
    {
        return sceneReference.ScenePath;
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        HandleBeforeSerialize();
#endif
    }

    public void OnAfterDeserialize()
    {
#if UNITY_EDITOR
        EditorApplication.update += HandleAfterDeserialize;
#endif
    }



#if UNITY_EDITOR
    private SceneAsset GetSceneAssetFromPath()
    {
        if (string.IsNullOrEmpty(scenePath))
            return null;
        return AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
    }

    private string GetScenePathFromAsset()
    {
        if (sceneAsset == null)
            return string.Empty;
        return AssetDatabase.GetAssetPath(sceneAsset);
    }

    private void HandleBeforeSerialize()
    {
        if (IsValidSceneAsset == false && string.IsNullOrEmpty(scenePath) == false)
        {
            sceneAsset = GetSceneAssetFromPath();
            if (sceneAsset == null)
                scenePath = string.Empty;

            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
        else
        {
            scenePath = GetScenePathFromAsset();
        }
    }

    private void HandleAfterDeserialize()
    {
        EditorApplication.update -= HandleAfterDeserialize;
        if (IsValidSceneAsset)
            return;

        if (string.IsNullOrEmpty(scenePath) == false)
        {
            sceneAsset = GetSceneAssetFromPath();
            if (sceneAsset == null)
                scenePath = string.Empty;

            if (Application.isPlaying == false)
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
    }
#endif
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneReference))]
public class SceneReferencePropertyDrawer : PropertyDrawer
{
    const string sceneAssetPropertyString = "sceneAsset";
    const string scenePathPropertyString = "scenePath";

    static readonly RectOffset boxPadding = EditorStyles.helpBox.padding;
    static readonly float padSize = 2f;
    static readonly float lineHeight = EditorGUIUtility.singleLineHeight;
    static readonly float paddedLine = lineHeight + padSize;
    static readonly float footerHeight = 10f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var sceneAssetProperty = GetSceneAssetProperty(property);

        position.height -= footerHeight;
        GUI.Box(EditorGUI.IndentedRect(position), GUIContent.none, EditorStyles.helpBox);
        position = boxPadding.Remove(position);
        position.height = lineHeight;

        label.tooltip = "The actual Scene Asset reference.\nOn serialize this is also stored as the asset's path.";

        EditorGUI.BeginProperty(position, GUIContent.none, property);
        EditorGUI.BeginChangeCheck();
        int sceneControlID = GUIUtility.GetControlID(FocusType.Passive);
        var selectedObject = EditorGUI.ObjectField(position, label, sceneAssetProperty.objectReferenceValue, typeof(SceneAsset), false);
        BuildUtils.BuildScene buildScene = BuildUtils.GetBuildScene(selectedObject);

        if (EditorGUI.EndChangeCheck())
        {
            sceneAssetProperty.objectReferenceValue = selectedObject;

            if (buildScene.scene == null)
                GetScenePathProperty(property).stringValue = string.Empty;
        }
        position.y += paddedLine;

        if (buildScene.assetGUID.Empty() == false)
        {
            DrawSceneInfoGUI(position, buildScene, sceneControlID + 1);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lines = 2;
        SerializedProperty sceneAssetProperty = GetSceneAssetProperty(property);
        if (sceneAssetProperty.objectReferenceValue == null)
            lines = 1;

        return boxPadding.vertical + lineHeight * lines + padSize * (lines - 1) + footerHeight;
    }

    private void DrawSceneInfoGUI(Rect position, BuildUtils.BuildScene buildScene, int sceneControlID)
    {
        bool readOnly = BuildUtils.IsReadOnly();
        string readOnlyWarning = readOnly ? "\n\nWARNING: Build Settings is not checked out and so cannot be modified." : "";

        GUIContent iconContent = new GUIContent();
        GUIContent labelContent = new GUIContent();

        if (buildScene.buildIndex == -1)
        {
            iconContent = EditorGUIUtility.IconContent("d_winbtn_mac_close");
            labelContent.text = "NOT In Build";
            labelContent.tooltip = "This scene is NOT in build settings.\nIt will be NOT included in builds.";
        }
        else if (buildScene.scene.enabled)
        {
            iconContent = EditorGUIUtility.IconContent("d_winbtn_mac_max");
            labelContent.text = "BuildIndex: " + buildScene.buildIndex;
            labelContent.tooltip = "This scene is in build settings and ENABLED.\nIt will be included in builds." + readOnlyWarning;
        }
        else
        {
            iconContent = EditorGUIUtility.IconContent("d_winbtn_mac_min");
            labelContent.text = "BuildIndex: " + buildScene.buildIndex;
            labelContent.tooltip = "This scene is in build settings and DISABLED.\nIt will be NOT included in builds.";
        }

        using (new EditorGUI.DisabledScope(readOnly))
        {
            Rect labelRect = DrawUtils.GetLabelRect(position);
            Rect iconRect = labelRect;
            iconRect.width = iconContent.image.width + padSize;
            labelRect.width -= iconRect.width;
            labelRect.x += iconRect.width;
            EditorGUI.PrefixLabel(iconRect, sceneControlID, iconContent);
            EditorGUI.PrefixLabel(labelRect, sceneControlID, labelContent);
        }

        Rect buttonRect = DrawUtils.GetFieldRect(position);
        buttonRect.width = (buttonRect.width) / 3;

        string tooltipMsg = "";
        using (new EditorGUI.DisabledScope(readOnly))
        {
            if (buildScene.buildIndex == -1)
            {
                buttonRect.width *= 2;
                int addIndex = EditorBuildSettings.scenes.Length;
                tooltipMsg = "Add this scene to build settings. It will be appended to the end of the build scenes as buildIndex: " + addIndex + "." + readOnlyWarning;
                if (DrawUtils.ButtonHelper(buttonRect, "Add...", "Add (buildIndex " + addIndex + ")", EditorStyles.miniButtonLeft, tooltipMsg))
                    BuildUtils.AddBuildScene(buildScene);
                buttonRect.width /= 2;
                buttonRect.x += buttonRect.width;
            }
            else
            {
                bool isEnabled = buildScene.scene.enabled;
                string stateString = isEnabled ? "Disable" : "Enable";
                tooltipMsg = stateString + " this scene in build settings.\n" + (isEnabled ? "It will no longer be included in builds" : "It will be included in builds") + "." + readOnlyWarning;

                if (DrawUtils.ButtonHelper(buttonRect, stateString, stateString + " In Build", EditorStyles.miniButtonLeft, tooltipMsg))
                    BuildUtils.SetBuildSceneState(buildScene, !isEnabled);
                buttonRect.x += buttonRect.width;

                tooltipMsg = "Completely remove this scene from build settings.\nYou will need to add it again for it to be included in builds!" + readOnlyWarning;
                if (DrawUtils.ButtonHelper(buttonRect, "Remove...", "Remove from Build", EditorStyles.miniButtonMid, tooltipMsg))
                    BuildUtils.RemoveBuildScene(buildScene);
            }
        }

        buttonRect.x += buttonRect.width;

        tooltipMsg = "Open the 'Build Settings' Window for managing scenes." + readOnlyWarning;
        if (DrawUtils.ButtonHelper(buttonRect, "Settings", "Build Settings", EditorStyles.miniButtonRight, tooltipMsg))
        {
            BuildUtils.OpenBuildSettings();
        }

    }

    static SerializedProperty GetSceneAssetProperty(SerializedProperty property)
    {
        return property.FindPropertyRelative(sceneAssetPropertyString);
    }

    static SerializedProperty GetScenePathProperty(SerializedProperty property)
    {
        return property.FindPropertyRelative(scenePathPropertyString);
    }

    private static class DrawUtils
    {
        static public bool ButtonHelper(Rect position, string msgShort, string msgLong, GUIStyle style, string tooltip = null)
        {
            GUIContent content = new GUIContent(msgLong);
            content.tooltip = tooltip;

            float longWidth = style.CalcSize(content).x;
            if (longWidth > position.width)
                content.text = msgShort;

            return GUI.Button(position, content, style);
        }

        static public Rect GetFieldRect(Rect position)
        {
            position.width -= EditorGUIUtility.labelWidth;
            position.x += EditorGUIUtility.labelWidth;
            return position;
        }

        static public Rect GetLabelRect(Rect position)
        {
            position.width = EditorGUIUtility.labelWidth - padSize;
            return position;
        }
    }

    static private class BuildUtils
    {
        public static float minCheckWait = 3;

        static float lastTimeChecked = 0;
        static bool cachedReadonlyVal = true;

        public struct BuildScene
        {
            public int buildIndex;
            public GUID assetGUID;
            public string assetPath;
            public EditorBuildSettingsScene scene;
        }

        static public bool IsReadOnly()
        {
            float curTime = Time.realtimeSinceStartup;
            float timeSinceLastCheck = curTime - lastTimeChecked;

            if (timeSinceLastCheck > minCheckWait)
            {
                lastTimeChecked = curTime;
                cachedReadonlyVal = QueryBuildSettingsStatus();
            }

            return cachedReadonlyVal;
        }

        static private bool QueryBuildSettingsStatus()
        {
            if (UnityEditor.VersionControl.Provider.enabled == false)
                return false;

            if (UnityEditor.VersionControl.Provider.hasCheckoutSupport == false)
                return false;


            var status = UnityEditor.VersionControl.Provider.Status("ProjectSettings/EditorBuildSettings.asset", false);
            status.Wait();

            if (status.assetList == null || status.assetList.Count != 1)
                return true;

            if (status.assetList[0].IsState(UnityEditor.VersionControl.Asset.States.CheckedOutLocal))
                return false;

            return true;
        }

        static public BuildScene GetBuildScene(Object sceneObject)
        {
            BuildScene entry = new BuildScene()
            {
                buildIndex = -1,
                assetGUID = new GUID(string.Empty)
            };

            if (sceneObject as SceneAsset == null)
                return entry;

            entry.assetPath = AssetDatabase.GetAssetPath(sceneObject);
            entry.assetGUID = new GUID(AssetDatabase.AssetPathToGUID(entry.assetPath));

            for (int index = 0; index < EditorBuildSettings.scenes.Length; ++index)
            {
                if (entry.assetGUID.Equals(EditorBuildSettings.scenes[index].guid))
                {
                    entry.scene = EditorBuildSettings.scenes[index];
                    entry.buildIndex = index;
                    return entry;
                }
            }

            return entry;
        }

        static public void SetBuildSceneState(BuildScene buildScene, bool enabled)
        {
            bool modified = false;
            EditorBuildSettingsScene[] scenesToModify = EditorBuildSettings.scenes;
            foreach (var curScene in scenesToModify)
            {
                if (curScene.guid.Equals(buildScene.assetGUID))
                {
                    curScene.enabled = enabled;
                    modified = true;
                    break;
                }
            }
            if (modified)
                EditorBuildSettings.scenes = scenesToModify;
        }

        static public void AddBuildScene(BuildScene buildScene, bool force = false, bool enabled = true)
        {
            if (force == false)
            {
                int selection = EditorUtility.DisplayDialogComplex(
                    "Add Scene To Build",
                    "You are about to add scene at " + buildScene.assetPath + " To the Build Settings.",
                    "Add as Enabled",       // option 0
                    "Add as Disabled",      // option 1
                    "Cancel (do nothing)"); // option 2

                switch (selection)
                {
                    case 0: // enabled
                        enabled = true;
                        break;
                    case 1: // disabled
                        enabled = false;
                        break;
                    default:
                    case 2: // cancel
                        return;
                }
            }

            EditorBuildSettingsScene newScene = new EditorBuildSettingsScene(buildScene.assetGUID, enabled);
            List<EditorBuildSettingsScene> tempScenes = EditorBuildSettings.scenes.ToList();
            tempScenes.Add(newScene);
            EditorBuildSettings.scenes = tempScenes.ToArray();
        }

        static public void RemoveBuildScene(BuildScene buildScene, bool force = false)
        {
            bool onlyDisable = false;
            if (force == false)
            {
                int selection = -1;

                string title = "Remove Scene From Build";
                string details = string.Format("You are about to remove the following scene from build settings:\n    {0}\n    buildIndex: {1}\n\n{2}",
                                buildScene.assetPath, buildScene.buildIndex,
                                "This will modify build settings, but the scene asset will remain untouched.");
                string confirm = "Remove From Build";
                string alt = "Just Disable";
                string cancel = "Cancel (do nothing)";

                if (buildScene.scene.enabled)
                {
                    details += "\n\nIf you want, you can also just disable it instead.";
                    selection = EditorUtility.DisplayDialogComplex(title, details, confirm, alt, cancel);
                }
                else
                {
                    selection = EditorUtility.DisplayDialog(title, details, confirm, cancel) ? 0 : 2;
                }

                switch (selection)
                {
                    case 0: // remove
                        break;
                    case 1: // disable
                        onlyDisable = true;
                        break;
                    default:
                    case 2: // cancel
                        return;
                }
            }

            if (onlyDisable)
            {
                SetBuildSceneState(buildScene, false);
            }
            else
            {
                List<EditorBuildSettingsScene> tempScenes = EditorBuildSettings.scenes.ToList();
                tempScenes.RemoveAll(scene => scene.guid.Equals(buildScene.assetGUID));
                EditorBuildSettings.scenes = tempScenes.ToArray();
            }
        }

        static public void OpenBuildSettings()
        {
            EditorWindow.GetWindow(typeof(BuildPlayerWindow));
        }
    }
}

#endif
