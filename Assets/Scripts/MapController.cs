using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TerrainUtils;

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
        CheckChunks();
        OptimizeChunks();
    }

    void CheckChunks()
    {
        if (!CurrentChunk) return;
        if (!Player.activeSelf) return;
        if (!pm.isActiveAndEnabled) return;

        if (pm.MoveDirection.x > 0 && pm.MoveDirection.y == 0)
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.Find("Right").position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.Find("Right").position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.x < 0 && pm.MoveDirection.y == 0)
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.Find("Left").position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.Find("Left").position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.y > 0 && pm.MoveDirection.x == 0)
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.Find("Up").position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.Find("Up").position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.y < 0 && pm.MoveDirection.x == 0)
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.Find("Down").position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.Find("Down").position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.x > 0 && pm.MoveDirection.y > 0)
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.Find("Up-Right").position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.Find("Up-Right").position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.x > 0 && pm.MoveDirection.y < 0)
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.Find("Down-Right").position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.Find("Down-Right").position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.x < 0 && pm.MoveDirection.y > 0)
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.Find("Up-Left").position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.Find("Up-Left").position;
                SpawnChunk();
            }
        }
        else if (pm.MoveDirection.x < 0 && pm.MoveDirection.y < 0)
        {
            if (!Physics2D.OverlapCircle(CurrentChunk.transform.Find("Down-Left").position, CheckerRadius, TerrainMask))
            {
                NewTerrainPosition = CurrentChunk.transform.Find("Down-Left").position;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        int Rand = Random.Range(0,TerrainChunks.Count);
        LatestChunk = Instantiate(TerrainChunks[Rand], NewTerrainPosition, Quaternion.identity);
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
