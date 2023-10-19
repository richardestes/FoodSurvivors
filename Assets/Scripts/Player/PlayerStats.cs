using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    public List<GameObject> spawnedWeapons; 
    
    // Current Stats
    [HideInInspector]public float currentHealth;
    [HideInInspector]public float currentRecovery;
    [HideInInspector]public float currentMoveSpeed;
    [HideInInspector]public float currentMight;
    [HideInInspector]public float currentProjectileSpeed;
    [HideInInspector]public float currentMagnet;

    // Experience and Leveling
    [Header("Experience/Level")]
    public int Experience = 0;
    public int Level = 1;
    public int ExperienceCap;
    
    // I-Frames
    [Header("IFrames")] public float invincibilityDuration;
    private float invincibilityTimer;
    private bool isInvincible;

    [System.Serializable] // Allows for editing in inspector
    public class LevelRange
    {
        public int StartLevel;
        public int EndLevel;
        public int ExperienceCapIncrease;
    }
    public List<LevelRange> LevelRanges;
    
    private void Start()
    {
        // Without this, the player start with an experience cap of 0 and can level up immediately
        ExperienceCap = LevelRanges[0].ExperienceCapIncrease;
    }

    private void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();
        
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;
        
        SpawnWeapon(characterData.StartingWeapon);
    }

    private void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }
        Recover();
    }

    public void IncreaseExperience(int amount)
    {
        Experience += amount;
        CheckLevel();
    }

    public void CheckLevel()
    {
        if (Experience >= ExperienceCap)
        {
            Level++;
            Experience -= ExperienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in LevelRanges)
            {
                if (Level >= range.StartLevel && Level <= range.EndLevel)
                {
                    experienceCapIncrease = range.ExperienceCapIncrease;
                    break;
                }
            }
            ExperienceCap += experienceCapIncrease;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (isInvincible) return;
        currentHealth -= dmg;
        invincibilityTimer = invincibilityDuration;
        isInvincible = true;
        
        if (currentHealth <= 0f)
        {
            Kill();
        }
    }

    public void RestoreHealth(float amount)
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;
            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void Kill()
    {
        print("player is dead");
    }

    public void Recover()
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;
        }

        if (currentHealth > characterData.MaxHealth) // Check to prevent floating values from exceeding MaxHealth
        {
            currentHealth = characterData.MaxHealth;
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        // Spawn starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); // Make weapon a child of the Player
        spawnedWeapons.Add(spawnedWeapon);
    }
}
