using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerUtils : MonoBehaviour
{
    public void DisableObjectOnEnter(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void EnableObjectOnEnter(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
