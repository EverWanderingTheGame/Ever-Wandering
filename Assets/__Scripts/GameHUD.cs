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
    public string[] BlacklistedScenes;

    public static bool isPaused;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !BlacklistedScenes.Contains(SceneManager.GetActiveScene().name))
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
        pauseMenuFX.SetActive(true);
        PlayerHUD.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Pause");

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        pauseMenuFX.SetActive(false);
        PlayerHUD.SetActive(true);
        FindObjectOfType<AudioManager>().Stop("Pause");

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
