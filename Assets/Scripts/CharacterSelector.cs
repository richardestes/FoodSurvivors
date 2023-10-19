using System;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{

    public static CharacterSelector instance;

    public CharacterScriptableObject characterData;

    // Singleton shit
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Extra " + this + " deleted");
            Destroy(gameObject);
        }
    }

    public static CharacterScriptableObject GetData()
    {
        return instance.characterData;
    }

    public void SelectCharacter(CharacterScriptableObject selectedChar)
    {
        characterData = selectedChar;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
