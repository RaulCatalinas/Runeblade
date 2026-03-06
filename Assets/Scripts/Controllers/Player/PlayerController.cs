using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController movementController;
    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform startPoint;

    private bool jumpRequested = false;
    private bool attackRequested = false;
    private bool dashRequested = false;
    private Vector3 lastCheckpointActivated;

    void Start()
    {
        lastCheckpointActivated = startPoint.position;
        gameObject.transform.position = startPoint.position;
    }

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            Debug.Log("Checkpoint activated");
            lastCheckpointActivated = collision.transform.position;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WorldLimit"))
        {
            Debug.Log("The player has gone beyond the boundaries of the world. Reappearing in the last safe position.");
            gameObject.transform.position = lastCheckpointActivated;
        }
    }
}
