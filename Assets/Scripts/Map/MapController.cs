using System;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private Vector3 playerLastPosition;
    public List<GameObject> TerrainChunks;
    public GameObject Player;
    public float CheckerRadius;
    public LayerMask TerrainMask;
    public GameObject CurrentChunk;

    [Header("Optimization")]
    public List<GameObject> SpawnedChunks;
    GameObject LatestChunk;
    public float MaxOptimizeDistance; // must be greater than length and width of tilemap
    float OptimizeDistance;
    float OptimizerCooldown;
    public float OptimizerCooldownDuration;

    void Start()
    {
        playerLastPosition = Player.transform.position;
    }

    void Update()
    {
        CheckChunks();
        OptimizeChunks();
    }
    

    void CheckChunks()
    {
        if (!CurrentChunk) return;

        Vector3 moveDir = Player.transform.position - playerLastPosition;
        playerLastPosition = Player.transform.position;

        string directionName = GetDirectionName(moveDir);
        CheckAndSpawnChunk(directionName);

        // Check additional adjacent directions for diagonal chunks
        if (directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Up");
        }
        if (directionName.Contains("Down"))
        {
            CheckAndSpawnChunk("Down");
        }
        if (directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Right");
        }
        if (directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Left");
        }
    }
    
    void CheckAndSpawnChunk(string direction)
    {
        // if (!CurrentChunk.transform) return;
        if (!Physics2D.OverlapCircle(CurrentChunk.transform.Find(direction).position, CheckerRadius, TerrainMask))
        {
            SpawnChunk(CurrentChunk.transform.Find(direction).position);
        }
    }

    void SpawnChunk(Vector3 spawnPosition)
    {
        int Rand = UnityEngine.Random.Range(0,TerrainChunks.Count);
        LatestChunk = Instantiate(TerrainChunks[Rand], spawnPosition, Quaternion.identity);
        SpawnedChunks.Add(LatestChunk);
    }

    void OptimizeChunks()
    {
        OptimizerCooldown -= Time.deltaTime;

        if (OptimizerCooldown <= 0f)
        {
            OptimizerCooldown = OptimizerCooldownDuration;
        }
        else return;

        foreach(GameObject Chunk in SpawnedChunks)
        {
            OptimizeDistance = Vector3.Distance(Player.transform.position, Chunk.transform.position);
            if (OptimizeDistance > MaxOptimizeDistance)
            {
                Chunk.SetActive(false);
            }
            else
            {
                Chunk.SetActive(true);
            }
        }
    }
    
    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;
        if (Mathf.Abs(direction.x) > Math.Abs(direction.y))
        {
            if (direction.y > 0.5f)
            {
                return direction.x > 0 ? "Right Up" : "Left Up";
            }
            else if (direction.y < -0.5f)
            {
                return direction.x > 0 ? "Right Down" : "Left Down";
            }
            else
            {
                return direction.x > 0 ? "Right" : "Left";
            }
        }
        else
        {
            if (direction.x > 0.5f)
            {
                return direction.y > 0 ? "Right Up" : "Right Down";
            }
            else if (direction.x < -0.5f)
            {
                return direction.y > 0 ? "Left Up" : "Left Down";
            }
            else
            {
                return direction.y > 0 ? "Up" : "Down";
            }
        }
    }
}
