using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public float LastHorizontalVector;
    [HideInInspector]
    public float LastVerticalVector;
    [HideInInspector]
    public Vector2 MoveDirection;
    [HideInInspector]
    public Vector2 lastMoveVector;

    // References
    Rigidbody2D _rigidBody;
    private PlayerStats _player;
    void Start()
    {
        _player = GetComponent<PlayerStats>();
        _rigidBody = GetComponent<Rigidbody2D>();
        lastMoveVector = new Vector2(1f, 0f); // default fire direction: right
    }

    void Update()
    {
        InputManagement();
    }

    void FixedUpdate() // not dependent on frame rate
    {
        Move();
    }

    void InputManagement()
    {
        if (GameManager.instance.IsGameOver) return;
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        MoveDirection = new Vector2(moveX, moveY).normalized;

        if (MoveDirection.x != 0)
        {
            LastHorizontalVector = MoveDirection.x;
            lastMoveVector = new Vector2(LastHorizontalVector, 0f);
        }
        if (MoveDirection.y != 0)
        {
            LastVerticalVector = MoveDirection.y;
            lastMoveVector = new Vector2(0f,LastVerticalVector);
        }

        if (MoveDirection.x != 0 && MoveDirection.y != 0)
        {
            lastMoveVector = new Vector2(LastHorizontalVector, LastVerticalVector);
        }
    }

    void Move()
    {
        if (GameManager.instance.IsGameOver) return;
        _rigidBody.velocity = new Vector2(MoveDirection.x * _player.CurrentMoveSpeed, MoveDirection.y * _player.CurrentMoveSpeed);
    }
}
