using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
    private Transform _player;
    private EnemySpawner _enemySpawner;
    
    // Current stats
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentDamage;

    public EnemyScriptableObject enemyData;
    public float DespawnDistance = 40f;

    [Header("Damage Feedback")]
    private Color _originalColor;
    private SpriteRenderer _spriteRenderer;
    private EnemyMovement _enemyMovement;
    public Color DamageColor = new Color(1, 1, 1, 1);
    public float DamageFlashDuration = 0.2f;
    public float DeathFadeTime = 0.6f;


    private void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    private void Start()
    {
        _player = FindObjectOfType<PlayerStats>().transform;
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _originalColor = _spriteRenderer.color;
    }

    IEnumerator DamageFlash()
    {
        _spriteRenderer.color = DamageColor;
        yield return new WaitForSeconds(DamageFlashDuration);
        _spriteRenderer.color = _originalColor;
    }

    IEnumerator KillFade()
    {
        // Wait for a single frame
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, originalAlpha = _spriteRenderer.color.a;
        
        // Fade alpha every frame
        while (t < DeathFadeTime)
        {
            yield return w;
            t += Time.deltaTime;
            //
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, (1 - t / DeathFadeTime) * originalAlpha);
        }
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration  = 0.2f)
    {
        currentHealth -= dmg;
        StartCoroutine(DamageFlash());
        
        // Create the text popup when enemy takes damage.
        if (dmg > 0)
            GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);

        if (knockbackForce > 0)
        {
            Vector2 direction = (Vector2)transform.position - sourcePosition; // Enemy position - source position = vector away from source position
            _enemyMovement.Knockback(direction.normalized * knockbackForce, knockbackDuration);
        }
        
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
        if (SceneController.GetCurrentSceneName() != "Game") return;
        _enemySpawner.OnEnemyKilled();
    }
    
    // THIS SHIT IS BROKEN
    // void FixedUpdate()
    //      {
    //          if (Vector2.Distance(transform.position, _player.position) >= DespawnDistance)
    //          {
    //              ReturnEnemy();
    //          }
    //      }
    
    // void ReturnEnemy()
    // {
    //     transform.position = _player.position +
    //                          _enemySpawner.SpawnPoints[Random.Range(0, _enemySpawner.SpawnPoints.Count)]
    //                              .position;
    // }
}
