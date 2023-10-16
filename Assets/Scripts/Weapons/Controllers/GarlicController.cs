using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicController : WeaponBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedGarlic = Instantiate(prefab);
        spawnedGarlic.transform.position = transform.position; // Assign to player position
        spawnedGarlic.transform.parent = transform;
    }
}
