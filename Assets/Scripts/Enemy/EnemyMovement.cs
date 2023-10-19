using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats _enemy;
    Transform _player;
    void Start()
    {
        _enemy = GetComponent<EnemyStats>();
        _player = FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _player.position, _enemy.currentMoveSpeed * Time.deltaTime);
    }
}
