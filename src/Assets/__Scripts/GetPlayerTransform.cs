using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerTransform : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        transform.position = player.transform.position;
    }
}
