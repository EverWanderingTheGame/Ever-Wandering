using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[DefaultExecutionOrder(-20000)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
