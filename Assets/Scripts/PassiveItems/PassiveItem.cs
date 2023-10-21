using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats playerStats;
    public PassiveItemScriptableObject PassiveItemData;
    
    
    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }

    protected virtual void ApplyModifier()
    {
        
    }
}
