using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static void TeleportPlayerToDefaultPosistion()
    {
        GameObject SpawnPoint = GameObject.FindWithTag("DefaultPlayerSpawn");
        GameObject Player = GameObject.Find("Player");
        if (SpawnPoint == null || Player == null)
        {
            Debug.Log("SpawnPoint NOT found");
            return;
        }
        Player.transform.position = SpawnPoint.transform.position;
    }
}
