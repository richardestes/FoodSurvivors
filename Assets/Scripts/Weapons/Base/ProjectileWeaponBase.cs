using UnityEngine;

// Base script for all projectiles
public class ProjectileWeaponBase : MonoBehaviour
{

    public WeaponScriptableObject weaponData;
    
    // Current stats
    public float currentDamage;
    public float currentSpeed;
    public float currentCooldownDuration;
    public int currentPierce;

    protected Vector3 direction;
    public float destroyAfterSeconds;
    private void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }
    protected virtual void Start()
    {
        Destroy(gameObject,destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirX = direction.x;
        float dirY = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        
        // Tedious direction checking for sprite flipping
        if (dirX < 0 && dirY == 0) // left
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (dirX == 0 && dirY < 0) // down
        {
            scale.y = scale.y * -1;
        } 
        else if (dirX == 0 && dirY > 0) // up
        {
            scale.x = scale.x * -1;
        }
        else if (dirX > 0 && dirY > 0) // up right
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 0f;
        }
        else if (dirX > 0 && dirY < 0) // down right
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }
        else if (dirX < 0 && dirY > 0) // up left
        {
            rotation.z = -90f;

        }
        else if (dirX < 0 && dirY < 0) // down left
        {
            rotation.z = 0f;
        }
        transform.localScale = scale;
        transform.localRotation = Quaternion.Euler(rotation);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage); // use currentDamage instead of weaponData.damage in case of any modifiers
            ReducePierce();
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps prop))
            {
                prop.TakeDamage(currentDamage);
                ReducePierce();
            }
        }
    }

    void ReducePierce()
    {
        currentPierce--;
        if (currentPierce <= 0) Destroy(gameObject);
    }
}
