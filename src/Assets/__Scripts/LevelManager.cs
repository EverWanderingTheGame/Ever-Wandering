using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] public GameObject _loaderCanvas;
    
    private Animator _loaderAnimator;

    public bool isLoading = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _loaderCanvas.SetActive(false);
        _loaderAnimator = _loaderCanvas.GetComponent<Animator>();
    }
    
    public async void LoadScene(string SceneName)
    {
        var scene = SceneManager.LoadSceneAsync(SceneName);
        scene.allowSceneActivation = false;
        
        isLoading = true;
        _loaderCanvas.SetActive(true);

        _loaderAnimator.SetTrigger("Start");
        await Task.Delay(500);

        do
        {
            await Task.Delay(100);
        } while (scene.progress < .9f);

        scene.allowSceneActivation = true;

        _loaderAnimator.SetTrigger("End");
        await Task.Delay(500);
        isLoading = false;
        await Task.Delay(500);

        _loaderCanvas.SetActive(false);
    }
}
