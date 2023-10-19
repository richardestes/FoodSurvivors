using System;
using Unity.VisualScripting;
using UnityEngine;

public class ExperienceGem : Pickup,ICollectible
{
    public int ExperienceGranted;
    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(ExperienceGranted);
    }
}
