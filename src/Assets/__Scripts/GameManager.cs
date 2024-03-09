using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
[DefaultExecutionOrder(-20000)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState = GameState.Playing;

    public GameObject player;
    public GameObject _loading;

    private Animator _loadingAnimator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _loadingAnimator = _loading.GetComponent<Animator>();
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

        if (Application.isPlaying)
        {
            if (gameState == GameState.Dialogue)
            {
                GameHUD.instance.unpauseable = true;
            }
            else
            {
                GameHUD.instance.unpauseable = false;
            }
        }
    }

    public void playerDead()
    {
        LevelManager.instance.LoadScene(Utils.getSceneNameFromSceneReference(FindObjectOfType<SceneSettings>().curScene));
    }
}

public enum GameState
{
    Playing,
    Paused,
    Dialogue,
    Prsentation
}