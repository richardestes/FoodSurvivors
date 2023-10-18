using UnityEngine;

public class ExperienceGem : MonoBehaviour,ICollectible
{
    public int ExperienceGranted;
    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(ExperienceGranted);
        Destroy(gameObject);
    }
}
