using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHUD : MonoBehaviour
{
    public static GameHUD instance;

    public GameObject pauseMenu;
    public GameObject pauseMenuFX;
    public GameObject PlayerHUD;
    public GameObject CameraFX;

    [HideInInspector] public bool unpauseable;
    //public static bool isPaused;

    void Awake()
    {
        if (instance == null) { instance = this; } 
        else { Destroy(gameObject); }
    }

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !unpauseable && GameManager.instance.gameState != GameState.Dialogue)
        {
            if(GameManager.instance.gameState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        CameraFX.SetActive(true);
        pauseMenuFX.SetActive(true);
        PlayerHUD.SetActive(false);
        Cursor.visible = true;
        AudioManager.instance.Play("Pause");

        Time.timeScale = 0f;
        GameManager.instance.gameState = GameState.Paused;
    }

    public void ResumeGame()
    {
        FindObjectOfType<SceneSettings>().applySceneSettings();

        pauseMenu.SetActive(false);
        pauseMenuFX.SetActive(false);
        AudioManager.instance.Stop("Pause");

        Time.timeScale = 1f;
        GameManager.instance.gameState = GameState.Playing;
    }

    public void GoToMainMenu()
    {
        ResumeGame();
        LevelManager.instance.LoadScene("MainMenu");
        PlayerHUD.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
