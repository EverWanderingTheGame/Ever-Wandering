using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ItemSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public float spawnRate = 1f;
    public float spawnRadius = 1f;
    public GameObject[] items;
    [Space]
    [Header("Kill Zone")]
    public GameObject killZone;
    public float killZoneY = 0f;

    private void Update()
    {
        if (killZone != null)
        {
            killZone.transform.position = new Vector3(transform.position.x, killZoneY, transform.position.z);
            killZone.GetComponent<BoxCollider2D>().size = new Vector2((spawnRadius * 8), 1f);
        }

        if (Random.value < spawnRate * Time.deltaTime && Application.isPlaying)

        {
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
        
        Instantiate(items[Random.Range(0, items.Length)], spawnPos, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
    }
}
