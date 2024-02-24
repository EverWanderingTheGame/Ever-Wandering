using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SceneReference StartScene;
    public SceneReference PresentationScene;

    public void PlayGame()
    {
        LevelManager.instance.LoadScene(Utils.getSceneNameFromSceneReference(StartScene));
    }

    public void Presentation()
    {
        LevelManager.instance.LoadScene(Utils.getSceneNameFromSceneReference(PresentationScene));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Fullscreen()
    {
        
    }
}
