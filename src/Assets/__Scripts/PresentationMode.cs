using Cinemachine;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
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
        if (Application.isPlaying)
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

            this.transform.position = posistions[posIndex].transform.position;
            Player.transform.position = posistions[posIndex].transform.position;

            posIndexMax = posistions.Length;
        }
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                posIndex++;
                if (posIndexMax == posIndex) posIndex = 0;

                animator.SetInteger("Scene", posIndex);

                Player.transform.position = posistions[posIndex].transform.position;

                GameManager.instance.player.SetActive(false);
            }
            else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                posIndex--;
                if (0 > posIndex) posIndex = 0;

                animator.SetInteger("Scene", posIndex);

                Player.transform.position = posistions[posIndex].transform.position;

                GameManager.instance.player.SetActive(false);
            }

            if (posIndex == 3)
            {
                GameManager.instance.player.SetActive(true);
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PresentationMode))]
public class PresentationModeEditor : Editor
{
    GameObject Player;
    GameObject[] positions;
    string[] positionName;
    int selected;
    int selectedOld;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!Application.isPlaying)
        {
            PresentationMode presentationMode = (PresentationMode)target;

            positions = presentationMode.posistions;
            positionName = new string[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                positionName[i] = positions[i].name;
            }

            GUILayout.Label("Editor Slide Position Tester", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            selected = EditorGUILayout.Popup("Select Position", selected, positionName);
            if (selected != selectedOld)
            {
                Player = GameObject.Find("Player");
                Player.transform.position = positions[selected].transform.position;
                selectedOld = selected;
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif
