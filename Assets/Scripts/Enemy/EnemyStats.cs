using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private Transform player;
    
    // Current stats
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentDamage;

    public EnemyScriptableObject enemyData;
    public float DespawnDistance = 20f;

    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    
    // THIS SHIT IS BROKEN
    void FixedUpdate()
    {
        // if (Vector2.Distance(transform.position, player.position) >= DespawnDistance)
        // {
        //     ReturnEnemy();
        // }
    }

    // This seems like a horrible idea
    void ReturnEnemy()
    {
        transform.position = player.position +
                             _enemySpawner.SpawnPoints[Random.Range(0, _enemySpawner.SpawnPoints.Count)]
                                 .position;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0f)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    public void OnDestroy()
    {
        _enemySpawner.OnEnemyKilled();
    }
}
