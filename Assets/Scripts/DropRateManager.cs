using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return; // do not call when the Editor exits Play
        float random = UnityEngine.Random.Range(0f, 100f);

        List<Drops> possibleDrops = new List<Drops>();

        foreach (Drops drop in drops)
        {
            if (random <= drop.dropRate)
            {
                possibleDrops.Add(drop);
            }
        }

        // Randomly choose from possible drops
        if (possibleDrops.Count > 0)
        {
            Drops drops = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(drops.itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
