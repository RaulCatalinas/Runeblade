using System;

using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("Platform")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private bool randomizeMovement = false;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float arrivalThreshold = 0.01f;

    [Header("Activation")]
    [SerializeField] private Transform player;

    private int currentWaypointIndex = 0;
    private bool goingForward = true;
    private RigidbodyConstraints2D horizontalConstraints;
    private RigidbodyConstraints2D verticalConstraints;
    private Vector2 lastPosition;
    private Vector2 platformVelocity;
    private Rigidbody2D rbPlayer;
    private bool isActive;

    void Awake()
    {
        if (waypoints.Length < 2)
        {
            throw new InvalidOperationException(
                "At least 2 waypoints are required for the platform to move."
            );
        }

        lastPosition = rb.position;
        horizontalConstraints =
            RigidbodyConstraints2D.FreezeRotation
            | RigidbodyConstraints2D.FreezePositionY;
        verticalConstraints =
            RigidbodyConstraints2D.FreezeRotation
            | RigidbodyConstraints2D.FreezePositionX;
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        platformVelocity = (rb.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = rb.position;

        if (!randomizeMovement) MoveOnOrder();
        else MoveRandomly();

        if (rbPlayer != null)
        {
            rbPlayer.position += platformVelocity * Time.fixedDeltaTime;
        }
    }

    void MoveOnOrder()
    {
        var target = waypoints[currentWaypointIndex].position;
        var newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);

        FreezeRigidbody(target);
        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, target) > arrivalThreshold) return;

        if (!goingForward)
        {
            if (currentWaypointIndex <= 0)
            {
                goingForward = true;
                currentWaypointIndex++;
            }
            else
            {
                currentWaypointIndex--;
            }

            return;
        }

        if (currentWaypointIndex >= waypoints.Length - 1)
        {
            goingForward = false;
            currentWaypointIndex--;
        }
        else
        {
            currentWaypointIndex++;
        }
    }

    void MoveRandomly() { }

    void FreezeRigidbody(Vector2 moveTarget)
    {
        var direction = moveTarget - rb.position;
        var moveHorizontally = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);

        rb.constraints = moveHorizontally ? horizontalConstraints : verticalConstraints;
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
        transform.position = waypoints[0].position;
        lastPosition = rb.position;

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
