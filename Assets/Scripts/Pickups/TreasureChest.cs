using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    private InventoryManager inventory;

    private void Start()
    {
        inventory = FindObjectOfType<InventoryManager>(); 
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Open();
            Destroy(gameObject);
        }
    }

    public void Open()
    {
        if (inventory.GetPossibleEvolutions().Count <= 0)
        {
            Debug.LogWarning("No Possible Evolutions");
            return;
        }
        // Randomly select weapon to evolve if multiple options are available
        WeaponEvolutionBlueprint toEvolve = inventory.GetPossibleEvolutions()[UnityEngine.Random.Range(0, inventory.GetPossibleEvolutions().Count)];
        inventory.EvolveWeapon(toEvolve);
    }
}
