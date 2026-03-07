using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlatformMovementController movementController;

    private bool isActive;
    private Rigidbody2D rbPlayer;

    void FixedUpdate()
    {
        if (!isActive) return;

        movementController.Move();

        if (rbPlayer != null)
            rbPlayer.position += movementController.Delta;
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
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;
        rbPlayer = null;
    }
}
