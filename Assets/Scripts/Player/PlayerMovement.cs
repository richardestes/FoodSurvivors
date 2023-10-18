using System.Collections;
using System.Collections.Generic;
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
    Rigidbody2D RigidBody;
    public CharacterScriptableObject characterData;
    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
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
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveY = Input.GetAxisRaw("Vertical");

        MoveDirection = new Vector2(MoveX, MoveY).normalized;

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
        RigidBody.velocity = new Vector2(MoveDirection.x * characterData.MoveSpeed, MoveDirection.y * characterData.MoveSpeed);
    }
}
