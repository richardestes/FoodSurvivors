using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;
    private InventoryManager inventory;
    
    // Current Stats
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;
    
    public int WeaponIndex;
    public int PassiveItemIndex;

    public GameObject firstPassiveItem, secondPassiveItem;

    public GameObject SecondWeaponTest;

    [Header("UI")]
    public Image HealthBar;
    public Image XPBar;
    public TMP_Text LevelText;
    
    #region Current Stat Properties
    public float CurrentHealth
    {
        get { return currentHealth;}
        set
        {
            if (currentHealth != value) // Update the real time value of this stat ONLY if it has changed
            {
                currentHealth = value;
                // When the game starts, there may be a delay causing the GameManager singleton to 
                // not yet be instantiated.
                if (GameManager.instance != null) //PERF: Maybe change this to IsUnityNull()
                {
                    GameManager.instance.CurrentHealthDisplay.text = "Health: " + currentHealth;
                }
            }
        }
    }
    
    public float CurrentRecovery
    {
        get { return currentRecovery;}
        set
        {
            if (currentRecovery != value) 
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
                }
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            if (currentMight != value)
            {
                currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMightDisplay.text = "Might: " + currentMight;
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentProjectileSpeedDisplay.text =
                        "Projectile Speed: " + currentProjectileSpeed;
                }
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            if (currentMagnet != value)
            {
                currentMagnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMagnetDisplay.text = "Magnet: " + currentMagnet;
                }
            }
        }
    }
    #endregion

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
        
        GameManager.instance.CurrentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.CurrentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.CurrentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
        GameManager.instance.CurrentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.CurrentProjectileSpeedDisplay.text =
            "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.CurrentMagnetDisplay.text = "Magnet: " + currentMagnet;
        
        GameManager.instance.AssignChosenCharacterUI(characterData);

        UpdateHealthBar();
        UpdateXPBar();
        UpdateLevelText();
    }

    private void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();
        
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;
        
        SpawnWeapon(characterData.StartingWeapon);
        // SpawnWeapon(SecondWeaponTest);
        // SpawnPassiveItem(firstPassiveItem);
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

    void UpdateXPBar()
    {
        XPBar.fillAmount = (float)Experience / ExperienceCap;
    }

    void UpdateLevelText()
    {
        LevelText.text = "LV " + Level.ToString();
    }
    
    public void IncreaseExperience(int amount)
    {
        Experience += amount;
        CheckLevelUp();
        UpdateXPBar();
    }

    public void CheckLevelUp()
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
            
            GameManager.instance.StartLevelUp();
            UpdateLevelText();
        }
    }

    public void TakeDamage(float dmg)
    {
        if (isInvincible) return;
        CurrentHealth -= dmg;
        invincibilityTimer = invincibilityDuration;
        isInvincible = true;
        
        if (CurrentHealth <= 0f)
        {
            Kill();
        }

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        HealthBar.fillAmount = CurrentHealth / characterData.MaxHealth;
    }

    public void RestoreHealth(float amount)
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    public void Kill()
    {
        if (!GameManager.instance.IsGameOver) // Ensures that this will only be called once
        {
            GameManager.instance.AssignResultingInventoryUI(inventory.WeaponUISlots, inventory.PassiveItemUISlots);
            GameManager.instance.AssignLevelReachedUI(Level);
            GameManager.instance.GameOver();
        }
    }

    public void Recover()
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;
        }

        if (CurrentHealth > characterData.MaxHealth) // Check to prevent floating values from exceeding MaxHealth
        {
            CurrentHealth = characterData.MaxHealth;
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
