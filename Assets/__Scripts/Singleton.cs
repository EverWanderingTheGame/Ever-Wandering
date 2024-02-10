using System.Linq;
using UnityEngine;

public class Singleton<T> : PrivateSingleton<T> where T : Component
{
    public static T Instance => instance;
    protected override void Awake()
    {
        base.Awake();

        if (instance == null)
        {
            instance = Object.FindObjectsOfType<T>().FirstOrDefault();
        }
    }
}