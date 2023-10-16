using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBase : MonoBehaviour
{

    public float destroyAfterSeconds;   
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

}
