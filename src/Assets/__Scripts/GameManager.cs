using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[DefaultExecutionOrder(-20000)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2)) // Take Screenshot
        {
#if !UNITY_EDITOR
            StartCoroutine(Utils.CoroutineScreenshot());
#else
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshot.png", 4);
            Debug.Log("Screenshot taken \n" + Application.dataPath + "/Screenshot.png");
#endif
        }
    }
}
