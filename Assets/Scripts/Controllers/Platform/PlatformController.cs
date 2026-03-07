using System;

using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private bool randomizeMovement = false;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float arrivalThreshold = 0.01f;

    private int currentWaypointIndex = 0;
    private bool goingForward = true;
    private RigidbodyConstraints2D horizontalConstraints;
    private RigidbodyConstraints2D verticalConstraints;
    private Vector2 lastPosition;
    private Vector2 platformDelta;
    private Rigidbody2D rbPlayer;
    private bool isActive;

    void Awake()
    {
        if (randomizeMovement && waypoints.Length < 3)
        {
            throw new InvalidOperationException(
                "At least 3 waypoints are required for the platform to move."
            );
        }
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

        lastPosition = rb.position;

        if (!randomizeMovement) MoveOnOrder();
        else MoveRandomly();

        platformDelta = rb.position - lastPosition;

        if (rbPlayer != null)
        {
            rbPlayer.position += platformDelta;
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

    private int currentTargetIndex = -1;

    void MoveRandomly()
    {
        if (currentTargetIndex == -1)
        {
            currentTargetIndex = UnityEngine.Random.Range(0, waypoints.Length);
        }

        var target = waypoints[currentTargetIndex].position;
        var newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, target) <= arrivalThreshold)
        {
            var newIndex = UnityEngine.Random.Range(0, waypoints.Length - 1);

            if (newIndex >= currentTargetIndex) newIndex++;

            currentTargetIndex = newIndex;
        }
    }

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
        lastPosition = rb.position;

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
