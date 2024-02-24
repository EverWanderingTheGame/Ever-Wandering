using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public bool endCredits = false;

    private bool once = false;

    void Update()
    {
        if (endCredits && !once)
        {
            LevelManager.instance.LoadScene(Utils.getSceneNameFromSceneReference(FindObjectOfType<SceneSettings>().nextScene));
            once = true;
        }
    }
}
