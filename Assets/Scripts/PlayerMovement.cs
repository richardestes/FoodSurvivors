using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float MovementSpeed;
    Rigidbody2D RigidBody;
    [HideInInspector]
    public float LastHorizontalVector;
    [HideInInspector]
    public float LastVerticalVector;
    [HideInInspector]
    public Vector2 MoveDirection;

    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
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
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveY = Input.GetAxisRaw("Vertical");

        MoveDirection = new Vector2(MoveX, MoveY).normalized;

        if (MoveDirection.x != 0)
        {
            LastHorizontalVector = MoveDirection.x;
            Debug.Log(MoveDirection.x);
        }
        if (MoveDirection.y != 0)
        {
            LastVerticalVector = MoveDirection.y;
            Debug.Log(MoveDirection.y);
        }
    }

    void Move()
    {
        RigidBody.velocity = new Vector2(MoveDirection.x * MovementSpeed, MoveDirection.y * MovementSpeed);
    }
}
