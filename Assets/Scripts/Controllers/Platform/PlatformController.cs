using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlatformMovementController movementController;

    private bool isActive;
    private Rigidbody2D rbPlayer;
    float playerContactTimer;
    const float CONTACT_GRACETIME = 0.1f;

    void FixedUpdate()
    {
        if (!isActive) return;

        movementController.Move();

        if (rbPlayer == null) return;

        rbPlayer.MovePosition(rbPlayer.position + movementController.Delta);

        if (movementController.Delta.y > 0)
        {
            rbPlayer.linearVelocityY = 0f;
        }

        playerContactTimer -= Time.fixedDeltaTime;

        if (playerContactTimer <= 0f)
        {
            rbPlayer = null;
        }
    }

    public void ActivatePlatform()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        isActive = true;

        Debug.Log($"[Platform] Activated: {gameObject.name}");
    }

    public void DeactivatePlatform()
    {
        rb.simulated = false;
        rb.bodyType = RigidbodyType2D.Static;
        isActive = false;
        movementController.ResetToStart();

        Debug.Log($"[Platform] Deactivated: {gameObject.name}");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        rbPlayer = collision.collider.GetComponent<Rigidbody2D>();
        playerContactTimer = CONTACT_GRACETIME;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        playerContactTimer = CONTACT_GRACETIME;
    }
}
