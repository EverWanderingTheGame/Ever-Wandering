using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PerformBootstrap
{
    const string SceneName = "Bootstrap";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
        {
            var candidate = SceneManager.GetSceneAt(sceneIndex);

            if (candidate.name == SceneName)
            {
                return;
            }
        }

        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }
}

public class Bootstrap : MonoBehaviour
{
    public static Bootstrap Instance { get; private set; } = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
