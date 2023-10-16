using System;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> TerrainChunks;
    public GameObject Player;
    public float CheckerRadius;
    Vector3 NewTerrainPosition;
    public LayerMask TerrainMask;
    public GameObject CurrentChunk;
    PlayerMovement pm;

    [Header("Optimization")]
    public List<GameObject> SpawnedChunks;
    GameObject LatestChunk;
    public float MaxOptimizeDistance; // must be greater than length and width of tilemap
    float OptimizeDistance;

    float OptimizerCooldown;
    public float OptimizerCooldownDuration;

    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        OptimizeChunks();
    }

    void FixedUpdate()
    {
        CheckChunks();
    }

    void CheckChunks()
    {
        if (!CurrentChunk) return;
        if (!Player.activeSelf) return;
        if (!pm.isActiveAndEnabled) return;
        
        // Direction Indexes
        // Up           0              
        // Left         1         
        // Right        2
        // Down         3
        // UpLeft       4
        // UpRight      5
        // DownLeft     6
        // DownRight    7       

        if (pm.MoveDirection.x > 0 && pm.MoveDirection.y > 0) // UpRight
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.GetChild(0).GetChild(5).position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.GetChild(0).GetChild(5).position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.x > 0 && pm.MoveDirection.y < 0) // DownRight
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.GetChild(0).GetChild(7).position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.GetChild(0).GetChild(7).position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.x < 0 && pm.MoveDirection.y > 0) // UpLeft
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.GetChild(0).GetChild(4).position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.GetChild(0).GetChild(4).position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.x < 0 && pm.MoveDirection.y < 0) // DownLeft
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.GetChild(0).GetChild(6).position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.GetChild(0).GetChild(6).position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.x > 0 && pm.MoveDirection.y == 0) // Right
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.GetChild(0).GetChild(2).position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.GetChild(0).GetChild(2).position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.x < 0 && pm.MoveDirection.y == 0) // Left
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.GetChild(0).GetChild(1).position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.GetChild(0).GetChild(1).position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.y > 0 && pm.MoveDirection.x == 0) // Up
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.GetChild(0).GetChild(0).position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.GetChild(0).GetChild(0).position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.y < 0 && pm.MoveDirection.x == 0) // Down
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.GetChild(0).GetChild(3).position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.GetChild(0).GetChild(3).position;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        //int Rand = Random.Range(0,TerrainChunks.Count);
        LatestChunk = Instantiate(TerrainChunks[0], NewTerrainPosition, Quaternion.identity);
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
}
