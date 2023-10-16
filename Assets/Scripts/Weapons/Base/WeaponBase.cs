using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// <summary>
// Base script for all weapons
// </summary>
public class WeaponBase : MonoBehaviour
{

    [Header("Weapon Stats")] public GameObject prefab;

    public float damage;

    public float speed;

    public float cooldownDuration;

    private float _currentCooldown;

    public int pierce;

    protected PlayerMovement Pm;
    protected virtual void Start()
    {
        Pm = FindObjectOfType<PlayerMovement>();
        _currentCooldown = cooldownDuration; // By default , this prevents a weapon from automatically attacking at pickup
    }

    protected virtual void Update()
    {
        _currentCooldown -= Time.deltaTime;
        if (_currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        _currentCooldown = cooldownDuration;
    }
}
