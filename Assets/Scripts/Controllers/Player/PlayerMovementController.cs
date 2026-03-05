using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float doubleJumpForce = 10f;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float dashSpeed = 10f;

    [Header("Coyote Time Settings")]
    [SerializeField] private float coyoteTime = 0.2f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerAnimator playerAnimator;

    [Header("Raycast Settings")]
    [SerializeField] private Transform raycastStartPoint;
    [SerializeField] private float raycastLength = 0.1f;

    private Vector2 moveInput;
    private float coyoteTimeCounter;
    private bool canDoubleJump = true;
    private bool isJumping = false;
    private Vector2 lastDirection = Vector2.right;

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;

        if (input.x != 0) lastDirection = input;

        playerAnimator.SetMoveAnimation(input.x != 0);
    }

    public void FixedTick()
    {
        Move();
        UpdateJumpState();
    }

    void Move()
    {
        var targetVelocityX = moveInput.x * moveSpeed;

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
            canDoubleJump = true;
            isJumping = false;
            playerAnimator.SetFallingAnimation(false);
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;

            if (rb.linearVelocityY < 0)
            {
                playerAnimator.SetJumpAnimation(false);
                playerAnimator.SetFallingAnimation(true);
            }
        }

        Debug.DrawRay(
            raycastStartPoint.position,
            Vector2.down * raycastLength,
            IsGrounded() ? Color.green : Color.red
        );
    }

    public void Jump()
    {
        if (coyoteTimeCounter > 0f && !isJumping)
        {
            isJumping = true;
            rb.linearVelocityY = jumpForce;
            playerAnimator.SetJumpAnimation(true);
        }
        else if (canDoubleJump && !IsGrounded())
        {
            rb.linearVelocityY = doubleJumpForce;
            canDoubleJump = false;
        }
    }

    public void Dash()
    {
        playerAnimator.SetDashAnimation(true);

        var dashDirection = moveInput.x != 0 ? moveInput.x : lastDirection.x;

        rb.linearVelocityX = Mathf.Sign(dashDirection) * dashSpeed;

        playerAnimator.SetDashAnimation(false);
    }

    bool IsGrounded()
    {
        var hit = Physics2D.Raycast(
            raycastStartPoint.position,
            Vector2.down,
            raycastLength,
            LayerMask.GetMask("Ground")
        );

        return hit.collider != null;
    }
}
