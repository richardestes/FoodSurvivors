using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject",menuName = "ScriptableObjects/PassiveItem")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    private float multiplier;
    public float Multiplier { get => multiplier; set => multiplier = value; }
    
    [SerializeField]
    private int level;
    public int Level { get => level; set => level = value; }

    [SerializeField]
    private GameObject nextLevelPrefab;
    public GameObject NextLevelPrefab { get => nextLevelPrefab; set => nextLevelPrefab = value; }
    
    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get=>icon; set => icon = value; }
}
