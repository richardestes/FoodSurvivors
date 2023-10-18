using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> PropSpawnPoints;
    public List<GameObject> PropPrefabs;

    void Start()
    {
        SpawnProps();
    }

    void Update()
    {
        
    }

    void SpawnProps()
    {
        foreach (GameObject SpawnPoint in PropSpawnPoints)
        {
            int RandomSeed = UnityEngine.Random.Range(0, PropSpawnPoints.Count);
            GameObject Prop = Instantiate(PropPrefabs[RandomSeed], SpawnPoint.transform.position, Quaternion.identity);
            Prop.transform.parent = SpawnPoint.transform;
        }
    }
}
