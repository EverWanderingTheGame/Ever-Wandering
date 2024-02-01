using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string SceneName = "DemoScene";

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneName);
        GameManager.TeleportPlayerToDefaultPosistion();
    }
}
