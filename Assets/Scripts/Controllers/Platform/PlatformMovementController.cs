using System;

using UnityEngine;

public class PlatformMovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private bool randomizeMovement = false;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float arrivalThreshold = 1.5f;

    private int currentWaypointIndex = 0;
    private bool goingForward = true;
    private int currentTargetIndex = -1;
    private RigidbodyConstraints2D horizontalConstraints;
    private RigidbodyConstraints2D verticalConstraints;

    public Vector2 Delta { get; private set; }
    public Vector2 LastPosition { get; private set; }

    void Awake()
    {
        if (randomizeMovement && waypoints.Length < 3)
        {
            throw new InvalidOperationException("At least 3 waypoints are required for random movement.");
        }
        if (waypoints.Length < 2)
        {
            throw new InvalidOperationException("At least 2 waypoints are required for the platform to move.");
        }

        horizontalConstraints =
            RigidbodyConstraints2D.FreezeRotation
            | RigidbodyConstraints2D.FreezePositionY;
        verticalConstraints =
            RigidbodyConstraints2D.FreezeRotation
            | RigidbodyConstraints2D.FreezePositionX;
        LastPosition = rb.position;
    }

    public void Move()
    {
        LastPosition = rb.position;

        if (!randomizeMovement) MoveOnOrder();
        else MoveRandomly();

        Delta = rb.position - LastPosition;
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

    public void ResetToStart()
    {
        transform.position = waypoints[0].position;
        LastPosition = rb.position;
        Delta = Vector2.zero;
    }
}
