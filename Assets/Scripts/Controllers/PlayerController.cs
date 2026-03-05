using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle,
    Running,
    Jumping,
    Falling,
    Attacking
}


public enum RaycastDirection
{
    Up,
    Down,
    Left,
    Right
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float doubleJumpForce = 10f;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float dashSpeed = 10f;

    [Header("Coyote Time Settings")]
    [SerializeField] private float coyoteTime = 0.2f;

    [Header("Player Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Raycast Settings")]
    [SerializeField] private RaycastDirection raycastDirection = RaycastDirection.Down;
    [SerializeField] private Transform raycastStartPoint;
    [SerializeField] private float raycastLength = 0.1f;

    [Header("Attack settings")]
    [SerializeField] private float damage = 3.5f;
    [SerializeField] private Transform sword;


    private Vector2 moveInput;
    private bool jumpRequested = false;
    private bool attackRequested = false;
    private bool dashRequested = false;
    private float coyoteTimeCounter = 0f;
    private bool canDoubleJump = false;
    private bool doubleJumpUnlocked = true;
    private bool isJumping = false;
    private Vector2 lastDirection = Vector2.right;

    void Start()
    {
        canDoubleJump = doubleJumpUnlocked;
    }

    void FixedUpdate()
    {
        Debug.DrawRay(
            raycastStartPoint.position,
            GetRaycastDirection() * raycastLength,
            IsGrounded() ? Color.green : Color.red
        );

        Move();
        UpdateJumpState();

        if (jumpRequested) Jump();
        if (attackRequested) Attack();
        if (dashRequested) Dash();
    }

    void Move()
    {
        var targetVelocityX = moveInput.x * moveSpeed;

        if (moveInput.x != 0) lastDirection = moveInput;

        rb.linearVelocityX = Mathf.MoveTowards(
            rb.linearVelocityX,
            targetVelocityX,
            acceleration * Time.fixedDeltaTime * 100f
        );
    }

    void UpdateJumpState()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            canDoubleJump = doubleJumpUnlocked;
            isJumping = false;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
    }

    void Jump()
    {
        if (coyoteTimeCounter > 0f && !isJumping)
        {
            isJumping = true;
            rb.linearVelocityY = jumpForce;
        }
        else if (canDoubleJump && !IsGrounded())
        {
            rb.linearVelocityY = doubleJumpForce;
            canDoubleJump = false;
        }

        jumpRequested = false;
    }

    void Attack()
    {
        Debug.Log("Attacking...");
    }

    void Dash()
    {
        dashRequested = false;

        if (moveInput.x < 0) rb.linearVelocityX = -dashSpeed;
        else if (moveInput.x > 0) rb.linearVelocityX = dashSpeed;
        else if (lastDirection.x < 0) rb.linearVelocityX = -dashSpeed;
        else if (lastDirection.x > 0) rb.linearVelocityX = dashSpeed;
        else rb.linearVelocityX = dashSpeed;
    }

    public void OnMove(InputValue value)
    {
        // TODO: Add running animation trigger here

        spriteRenderer.flipX = value.Get<Vector2>().x < 0;
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        // TODO: Add jump animation trigger here

        if (value.isPressed) jumpRequested = true;
    }

    public void OnAttack(InputValue value)
    {
        // TODO: Add attack animation trigger here

        if (value.isPressed) attackRequested = true;
    }

    public void OnDash(InputValue value)
    {
        // TODO: Add dash animation trigger here

        if (value.isPressed) dashRequested = true;
    }

    bool IsGrounded()
    {
        var hit = Physics2D.Raycast(
            raycastStartPoint.position,
            GetRaycastDirection(),
            raycastLength,
            LayerMask.GetMask("Ground")
        );

        return hit.collider != null;
    }

    Vector2 GetRaycastDirection()
    {
        return raycastDirection switch
        {
            RaycastDirection.Up => Vector2.up,
            RaycastDirection.Down => Vector2.down,
            RaycastDirection.Left => Vector2.left,
            RaycastDirection.Right => Vector2.right,
            _ => Vector2.down,
        };
    }
}
