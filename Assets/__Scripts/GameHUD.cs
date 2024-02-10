using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHUD : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseMenuFX;
    public GameObject PlayerHUD;
    public GameObject CameraFX;

    [HideInInspector] public bool unpauseable;
    public static bool isPaused;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !unpauseable)
        {
            if(isPaused)
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
        AudioManager.instance.Play("Pause");

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        CameraFX.SetActive(false);
        pauseMenuFX.SetActive(false);
        PlayerHUD.SetActive(true);
        AudioManager.instance.Stop("Pause");

        FindObjectOfType<SceneSettings>().applySceneSettings();

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("MainMenu");
        PlayerHUD.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
