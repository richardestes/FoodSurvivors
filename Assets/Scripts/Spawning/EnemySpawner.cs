using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public List<GameObject> enemySpawnPoints;
    public List<GameObject> enemyPrefabs;
    
    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        foreach (GameObject SpawnPoint in enemySpawnPoints)
        {
            int RandomSeed = UnityEngine.Random.Range(0, enemyPrefabs.Count);
            GameObject enemy = Instantiate(enemyPrefabs[RandomSeed], SpawnPoint.transform.position, Quaternion.identity);
        }
    }
}
