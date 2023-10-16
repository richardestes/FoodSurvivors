using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotBehavior : ProjectileWeaponBase
{
    private CarrotController cc;
    
    protected override void Start()
    {
        base.Start();
        cc = FindObjectOfType<CarrotController>();
    }

    void Update()
    {
        transform.position += direction * cc.speed * Time.deltaTime;
    }
}
