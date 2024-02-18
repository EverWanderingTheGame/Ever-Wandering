using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string StartScene = "DemoScene";
    public string PresentationScene = "Presentation";

    public void PlayGame()
    {
        LevelManager.instance.LoadScene(StartScene);
    }

    public void Presentation()
    {
        LevelManager.instance.LoadScene(PresentationScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
