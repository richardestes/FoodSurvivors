using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    
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
    }
    public void AddPassiveItem(int slotIndex, PassiveItem item)
    {
        PassiveItemSlots[slotIndex] = item;
        PassiveItemLevels[slotIndex] = item.PassiveItemData.Level;
        PassiveItemUISlots[slotIndex].enabled = true;
        PassiveItemUISlots[slotIndex].sprite = item.PassiveItemData.Icon;
    }

    public void LevelUpWeapon(int slotIndex)
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
        }
    }
    public void LevelUpPassiveItem(int slotIndex)
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
        }
    }
}
