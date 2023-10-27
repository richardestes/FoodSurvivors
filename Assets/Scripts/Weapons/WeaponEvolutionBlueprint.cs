using UnityEngine;

[CreateAssetMenu(fileName = "WeaponEvolutionBlueprint", menuName = "ScriptableObjects/WeaponEvolutionBlueprint")]
public class WeaponEvolutionBlueprint : ScriptableObject
{
    public WeaponScriptableObject BaseWeaponData;
    public PassiveItemScriptableObject CatalystPassiveItemData;
    public WeaponScriptableObject EvolvedWeaponData;
    public GameObject EvolvedWeapon;
}
