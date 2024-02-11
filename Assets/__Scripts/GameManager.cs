using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject Player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
