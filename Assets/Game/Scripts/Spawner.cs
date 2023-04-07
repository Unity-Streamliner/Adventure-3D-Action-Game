using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> spawnPointList;
    private bool hasSpawned;

    private void Awake()
    {
        var spawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoint>();
        spawnPointList = new List<SpawnPoint>(spawnPointArray);
    }

    public void SpawnCharacters()
    {
        if (hasSpawned) { return; }
        hasSpawned = true;

        foreach(SpawnPoint point in spawnPointList)
        {
            if (point.EnemyToSpawn != null)
            {
                GameObject spawnedGameObject = Instantiate(point.EnemyToSpawn, point.transform.position, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SpawnCharacters();
        }
    }
}