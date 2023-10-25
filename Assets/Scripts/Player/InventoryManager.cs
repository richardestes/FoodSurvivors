using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private PlayerStats player;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    [System.Serializable]
    public class WeaponUpgrade
    {
        public int WeaponUpgradeIndex;
        public GameObject InitialWeapon;
        public WeaponScriptableObject WeaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int PassiveItemUpgradeIndex;
        public GameObject InitialPassiveItem;
        public PassiveItemScriptableObject PassiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text UpgradeNameDisplay;
        public TMP_Text UpgradeDescriptionDisplay;
        public Image UpgradeIcon;
        public Button UpgradeButton;
    }

    public List<WeaponUpgrade> WeaponUpgradeOptions = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> PassiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    public List<UpgradeUI> UpgradeUIOptions = new List<UpgradeUI>();
    
    // This can be moved into a dictionary, however Unity doesn't show
    // dictionaries in the inspector by default
    public List<WeaponBase> WeaponSlots = new List<WeaponBase>(6);
    public int[] WeaponLevels = new int[6];
    public List<PassiveItem> PassiveItemSlots = new List<PassiveItem>(6);
    public int[] PassiveItemLevels = new int[6];
    
    public List<Image> WeaponUISlots = new List<Image>(6);
    public List<Image> PassiveItemUISlots = new List<Image>(6);

    public void AddWeapon(int slotIndex, WeaponBase weapon)
    {
        WeaponSlots[slotIndex] = weapon;
        WeaponLevels[slotIndex] = weapon.weaponData.Level;
        WeaponUISlots[slotIndex].enabled = true;
        WeaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        if (GameManager.instance != null & GameManager.instance.IsChoosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }
    public void AddPassiveItem(int slotIndex, PassiveItem item)
    {
        PassiveItemSlots[slotIndex] = item;
        PassiveItemLevels[slotIndex] = item.PassiveItemData.Level;
        PassiveItemUISlots[slotIndex].enabled = true;
        PassiveItemUISlots[slotIndex].sprite = item.PassiveItemData.Icon;
        
        if (GameManager.instance != null & GameManager.instance.IsChoosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if (WeaponSlots.Count > slotIndex)
        {
            WeaponBase weapon = WeaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogError("ERROR: No next level for: " + weapon.gameObject.name);
            }
            
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponBase>());
            Destroy(weapon.gameObject);
            WeaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponBase>().weaponData.Level;
            WeaponUpgradeOptions[upgradeIndex].WeaponData = upgradedWeapon.GetComponent<WeaponBase>().weaponData;
        }
        
        if (GameManager.instance != null & GameManager.instance.IsChoosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }
    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (PassiveItemSlots.Count > slotIndex)
        {
            PassiveItem item = PassiveItemSlots[slotIndex];
            
            if (!item.PassiveItemData.NextLevelPrefab)
            {
                Debug.LogError("ERROR: No next level for: " + item.gameObject.name);
            }
            
            GameObject upgradedItem =
                Instantiate(item.PassiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradedItem.GetComponent<PassiveItem>());
            Destroy(item.gameObject);
            PassiveItemLevels[slotIndex] = upgradedItem.GetComponent<PassiveItem>().PassiveItemData.Level;
            PassiveItemUpgradeOptions[upgradeIndex].PassiveItemData =
                upgradedItem.GetComponent<PassiveItem>().PassiveItemData;
        }
        
        if (GameManager.instance != null & GameManager.instance.IsChoosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    void ApplyUpgradeOptions()
    {
        // Use these lists to store available upgrades and remove any duplicates. 
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(WeaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(PassiveItemUpgradeOptions);
        
        foreach (var upgradeOption in UpgradeUIOptions)
        {
            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0) return;

            int upgradeType;
            
            // Handle OutOfRangeException error.
            if (availableWeaponUpgrades.Count == 0) upgradeType = 2;
            else if (availablePassiveItemUpgrades.Count == 0) upgradeType = 1;
            else upgradeType = UnityEngine.Random.Range(1, 3);
            
            if (upgradeType == 1)
            {
                WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[UnityEngine.Random.Range(0, availableWeaponUpgrades.Count)];
                availableWeaponUpgrades.Remove(chosenWeaponUpgrade); // remove duplicates
                if (chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool IsNewWeapon = false;
                    for (int i = 0; i < WeaponSlots.Count; i++)
                    {
                        if (WeaponSlots[i] != null && WeaponSlots[i].weaponData == chosenWeaponUpgrade.WeaponData)
                        {
                            IsNewWeapon = false;
                            if (!IsNewWeapon)
                            {
                                if (!chosenWeaponUpgrade.WeaponData.NextLevelPrefab) // Do not try to level up a weapon without a next level prefab
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break; 
                                }
                                upgradeOption.UpgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.WeaponUpgradeIndex));

                                upgradeOption.UpgradeDescriptionDisplay.text =
                                    chosenWeaponUpgrade.WeaponData.NextLevelPrefab.GetComponent<WeaponBase>().weaponData
                                        .Description;
                                upgradeOption.UpgradeNameDisplay.text = chosenWeaponUpgrade.WeaponData.NextLevelPrefab
                                    .GetComponent<WeaponBase>().weaponData.Name;
                            }

                            break;
                        }
                        else
                        {
                            IsNewWeapon = true;
                        }
                    }

                    if (IsNewWeapon)
                    {
                        upgradeOption.UpgradeButton.onClick.AddListener(() =>
                            player.SpawnWeapon(chosenWeaponUpgrade.InitialWeapon));

                        upgradeOption.UpgradeDescriptionDisplay.text = chosenWeaponUpgrade.WeaponData.Description;
                        upgradeOption.UpgradeNameDisplay.text = chosenWeaponUpgrade.WeaponData.Name;
                    }

                    upgradeOption.UpgradeIcon.sprite = chosenWeaponUpgrade.WeaponData.Icon;
                }
            }
            else if (upgradeType == 2)
            {
                Debug.Log("Choosing a Passive Item...");
                PassiveItemUpgrade chosenPassiveItemUpgrade =
                    availablePassiveItemUpgrades[UnityEngine.Random.Range(0, availablePassiveItemUpgrades.Count)];
                availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);
                if (chosenPassiveItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool IsNewPassiveItem = false;
                    for (int i = 0; i < PassiveItemSlots.Count; i++)
                    {
                        if (PassiveItemSlots[i] != null && PassiveItemSlots[i].PassiveItemData ==
                            chosenPassiveItemUpgrade.PassiveItemData)
                        {
                            IsNewPassiveItem = false;
                            if (!IsNewPassiveItem)
                            {
                                if (!chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }
                                upgradeOption.UpgradeButton.onClick.AddListener(()=> LevelUpPassiveItem(i, chosenPassiveItemUpgrade.PassiveItemUpgradeIndex));
                                    
                                upgradeOption.UpgradeDescriptionDisplay.text =
                                    chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().PassiveItemData
                                        .Description;
                                upgradeOption.UpgradeNameDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab
                                    .GetComponent<PassiveItem>().PassiveItemData.Name;                                }

                            break;
                        }
                        else
                        {
                            IsNewPassiveItem = true;
                        }
                    }
                    if (IsNewPassiveItem)
                    {
                        upgradeOption.UpgradeButton.onClick.AddListener(()=> player.SpawnPassiveItem(chosenPassiveItemUpgrade.InitialPassiveItem));

                        upgradeOption.UpgradeDescriptionDisplay.text =
                            chosenPassiveItemUpgrade.PassiveItemData.Description;
                        upgradeOption.UpgradeNameDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.Name;
                            
                    }
                    upgradeOption.UpgradeIcon.sprite = chosenPassiveItemUpgrade.PassiveItemData.Icon;
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach (var upgradeOptions in UpgradeUIOptions)
        {
            upgradeOptions.UpgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOptions);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.UpgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.UpgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
}
