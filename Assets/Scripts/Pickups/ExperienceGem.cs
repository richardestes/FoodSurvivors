using System;
using Unity.VisualScripting;
using UnityEngine;

public class ExperienceGem : Pickup
{
    public int ExperienceGranted;
    public override void Collect()
    {
        if (hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(ExperienceGranted);
    }
}
