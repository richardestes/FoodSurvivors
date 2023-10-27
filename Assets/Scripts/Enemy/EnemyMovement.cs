using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats _enemy;
    private Transform _player;

    private Vector2 _knockbackVelocity;
    private float _knockbackDuration;
    
    void Start()
    {
        _enemy = GetComponent<EnemyStats>();
        _player = FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
        // Process knockback
        if (_knockbackDuration > 0)
        {
            transform.position += (Vector3)_knockbackVelocity * Time.deltaTime;
            _knockbackDuration -= Time.deltaTime;
        }
        else
        {
            // Move towards player
            transform.position = Vector2.MoveTowards(transform.position, _player.position, _enemy.currentMoveSpeed * Time.deltaTime);
        }
    }

    public void Knockback(Vector2 velocity, float duration)
    {
        if (_knockbackDuration > 0) return;
        _knockbackVelocity = velocity;
        _knockbackDuration = duration;
    }
}
