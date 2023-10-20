using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Transform player;
    [Header("Spawner Attributes")] private float spawnTimer;
    
    public List<Wave> Waves;
    public int CurrentWaveIndex;
    public float WaveInterval;
    public int EnemiesAlive;
    public int MaxEnemiesAllowed; // Max enemies to be spawned on map
    public bool MaxEnemiesReached = false;

    [Header("Spawn Positions")] public List<Transform> SpawnPoints;
    
    [System.Serializable]
    public class EnemyGroup
    {
        public string EnemyName;
        public int EnemyCount; // Number of enemies to spawn in wave
        public int SpawnCount; // number of enemies of this type already spawned
        public GameObject EnemyPrefab;
    }
    
    [System.Serializable]
    public class Wave
    {
        public string WaveName;
        public List<EnemyGroup> EnemyGroups;
        public int WaveQuota;
        public int SpawnInterval;
        public int NumberOfSpawnedEnemies;
    }
    
    private void Start()
    {
        player = FindFirstObjectByType<PlayerStats>().transform;
        CalculateWaveQuota();
        //firstWave();
    }

    private void Update()
    {
        //Check if the wave has ended and the next wave should start
        if (CurrentWaveIndex < Waves.Count && Waves[CurrentWaveIndex].NumberOfSpawnedEnemies == 0)
        {
            StartCoroutine(BeginNextWave());
        }
        
        spawnTimer += Time.deltaTime;
        
        // Check if it's time to spawn next enemy
        if (spawnTimer >= Waves[CurrentWaveIndex].SpawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }
    void firstWave()
    {
        if (CurrentWaveIndex < Waves.Count && Waves[CurrentWaveIndex].NumberOfSpawnedEnemies == 0)
        {
            StartCoroutine(BeginNextWave());
        }
        spawnTimer = 0f;
        SpawnEnemies();
    }

    IEnumerator BeginNextWave()
    {
        // Wait for WaveInterval before spawning a new wave
        yield return new WaitForSeconds(WaveInterval);
        // Check if there are more waves to start
        if (CurrentWaveIndex < Waves.Count - 1)
        {
            CurrentWaveIndex++;
            CalculateWaveQuota();
        }
    }
    
    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        
        foreach (var enemyGroup in Waves[CurrentWaveIndex].EnemyGroups)
        {
            currentWaveQuota += enemyGroup.EnemyCount; // accumulates total number of enemies to spawn
        }

        Waves[CurrentWaveIndex].WaveQuota = currentWaveQuota;
        //Debug.LogWarning(currentWaveQuota);
    }
    
    void SpawnEnemies()
    {
        if (Waves[CurrentWaveIndex].NumberOfSpawnedEnemies < Waves[CurrentWaveIndex].WaveQuota && !MaxEnemiesReached) // Check if minimum number of enemies in current wave have been spawned
        { 
            foreach (var enemyGroup in Waves[CurrentWaveIndex].EnemyGroups) // Spawn each type of enemy until the quota is filled
            {
                if (enemyGroup.SpawnCount < enemyGroup.EnemyCount) // Check if minimum number of enemies of this type has been spawned
                {
                    // Limit amount of enemies to spawn
                    if (EnemiesAlive >= MaxEnemiesAllowed)
                    {
                        MaxEnemiesReached = true;
                        return;
                    }

                    Instantiate(enemyGroup.EnemyPrefab,
                        player.position + SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count)].position,
                        Quaternion.identity);
                    
                    enemyGroup.SpawnCount++;
                    Waves[CurrentWaveIndex].NumberOfSpawnedEnemies++;
                    EnemiesAlive++;
                }
            }
        }

        // Reset flag if number of alive enemies has dropped below maximum amount
        if (EnemiesAlive < MaxEnemiesAllowed)
        {
            MaxEnemiesReached = false;
        }
    }
    
    public void OnEnemyKilled()
    {
        EnemiesAlive--;
    }
}
