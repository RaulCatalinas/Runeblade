using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController movementController;
    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool jumpRequested = false;
    private bool attackRequested = false;
    private bool dashRequested = false;

    void FixedUpdate()
    {
        movementController.FixedTick();

        if (jumpRequested)
        {
            movementController.Jump();
            jumpRequested = false;
        }

        if (attackRequested)
        {
            combatController.Attack();
            attackRequested = false;
        }

        if (dashRequested)
        {
            movementController.Dash();
            dashRequested = false;
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();

        movementController.SetMoveInput(move);

        spriteRenderer.flipX = move.x < 0;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed) jumpRequested = true;
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed) attackRequested = true;
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed) dashRequested = true;
    }
}
