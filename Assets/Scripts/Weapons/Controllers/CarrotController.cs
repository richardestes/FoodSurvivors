using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotController : WeaponBase
{
    protected override void Start()
    { 
        base.Start();   
    }


    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedCarrot = Instantiate(prefab);
        spawnedCarrot.transform.position = transform.position;
        spawnedCarrot.GetComponent<CarrotBehavior>().DirectionChecker(Pm.lastMoveVector);
    }
    
}
