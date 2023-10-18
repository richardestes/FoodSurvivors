using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// <summary>
// Base script for all weapons
// </summary>
public class WeaponBase : MonoBehaviour
{

    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;

    protected PlayerMovement Pm;
    protected virtual void Start()
    {
        Pm = FindObjectOfType<PlayerMovement>();
        currentCooldown = weaponData.CooldownDuration; // By default , this prevents a weapon from automatically attacking at pickup
    }

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }
}
