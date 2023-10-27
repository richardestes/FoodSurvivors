using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehavior : MeleeWeaponBase
{
    private List<GameObject> markedEnemies;
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage(),transform.position);
            markedEnemies.Add(col.gameObject);
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps prop) && !markedEnemies.Contains(col.gameObject))
            {
                prop.TakeDamage(GetCurrentDamage());
                markedEnemies.Add(col.gameObject);
            }
        }
    }

}
