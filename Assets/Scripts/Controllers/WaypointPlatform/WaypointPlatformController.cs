using System;

using UnityEngine;

public class WaypointPlatformController : MonoBehaviour
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

    void Awake()
    {
        horizontalConstraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        verticalConstraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }

    void Start()
    {
        if (waypoints.Length < 2)
        {
            throw new InvalidOperationException("At least 2 waypoints are required for the platform to move.");
        }
    }

    void FixedUpdate()
    {


        if (!randomizeMovement) MoveOnOrder();
        else MoveRandomly();
    }

    void MoveOnOrder()
    {
        var target = waypoints[currentWaypointIndex].position;

        FreezeRigidbody(target);

        var newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);

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
}
