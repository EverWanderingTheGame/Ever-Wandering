using UnityEngine;

public class PrivateSingleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] protected bool dontDestroyOnLoad = true;
    [SerializeField] protected bool overrideInstance = false;

    protected static T instance;

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            if (overrideInstance)
            {
                Destroy(instance.gameObject);
                instance = this as T;
                Init();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this as T;

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
            Init();
        }
    }

    protected virtual void Init()
    {
    }

    protected virtual void OnDestroy()
    {
        if (instance == this && !overrideInstance)
        {
            instance = null;
        }
    }
}