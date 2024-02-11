using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public void PlayTheGame()
    {
        TrailManager.LightPower = 100;
        SceneManager.LoadScene("DemoScene");
    }

}
