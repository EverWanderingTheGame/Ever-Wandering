using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject BoxVFX;
    public GameObject ExitPortal;

    public void EnableVFX()
    {
        BoxVFX.SetActive(true);
    }

    public void DisableVFX()
    {
        BoxVFX.SetActive(false);
    }
    
    public void EnableExitPortal()
    {
        ExitPortal.SetActive(true);
    }

    public void DisableObjectOnEnter(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void EnableObjectOnEnter(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
}
