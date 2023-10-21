using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;
    private InventoryManager inventory;
    
    public int WeaponIndex;
    public int PassiveItemIndex;

    public GameObject firstPassiveItem, secondPassiveItem;

    public GameObject SecondWeaponTest;
    
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

        inventory = GetComponent<InventoryManager>();
        
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;
        
        SpawnWeapon(characterData.StartingWeapon);
        SpawnWeapon(SecondWeaponTest);
        SpawnPassiveItem(firstPassiveItem);
        SpawnPassiveItem(secondPassiveItem);
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
        if (WeaponIndex >= inventory.WeaponSlots.Count - 1)
        {
            Debug.LogError("ERROR: Tried to add weapon: " + weapon.name + " to a full inventory");
            return;
        }
        
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); // Make weapon a child of the Player
        inventory.AddWeapon(WeaponIndex, spawnedWeapon.GetComponent<WeaponBase>()); // Add to inventory

        WeaponIndex++;
    }

    public void SpawnPassiveItem(GameObject item)
    {
        if (PassiveItemIndex >= inventory.PassiveItemSlots.Count - 1)
        {
            Debug.LogError("ERROR: Tried to add passive item: " + item.name + " to a full inventory");
            return;
        }
        
        GameObject spawnedItem = Instantiate(item, transform.position, Quaternion.identity);
        spawnedItem.transform.SetParent(transform); // Make weapon a child of the Player
        inventory.AddPassiveItem(PassiveItemIndex, spawnedItem.GetComponent<PassiveItem>()); // Add to inventory

        PassiveItemIndex++;
    }
}
