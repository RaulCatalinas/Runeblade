using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyMovementController movementController;
    [SerializeField] private EnemyAttackController attackController;
    [SerializeField] private EnemyAnimator enemyAnimator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float activationRange = 20f;
    [SerializeField] private float desActivationRange = 25f;
    [SerializeField] private Transform player;
    [SerializeField] private CircleCollider2D circleCollider;

    private bool isActive = false;


    void Awake()
    {
        circleCollider.radius = enemyData.detectionRange;
    }

    void FixedUpdate()
    {
        if (
            !isActive
            && RangeDetector.IsInRange(
                transform.position,
                player.position,
                activationRange
            )
        )
        {
            Activate();
        }
        else if (
            isActive
            && RangeDetector.IsOutOfRange(
                transform.position,
                player.position,
                desActivationRange
            )
        )
        {
            DesActivate();
        }

        if (!isActive) return;

        movementController.Move();
    }

    void Activate()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        enemyAnimator.ActivateAnimator();
        isActive = true;

        Debug.Log($"[Enemy] Activated: {gameObject.name}");
    }

    void DesActivate()
    {
        rb.simulated = false;
        rb.bodyType = RigidbodyType2D.Static;
        enemyAnimator.DesActivateAnimator();
        isActive = false;

        Debug.Log($"[Enemy] Deactivated: {gameObject.name}");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        attackController.Attack();
    }
}
