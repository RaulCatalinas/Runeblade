using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyMovementController movementController;
    [SerializeField] private EnemyAttackController attackController;
    [SerializeField] private EnemyAnimator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float activationRange = 20f;
    [SerializeField] private float desActivationRange = 25f;
    [SerializeField] private Transform player;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private float knockbackForce;

    private bool isActive = false;
    private bool isAttacking = false;

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

        if (!isActive || isAttacking) return;

        movementController.Move();
    }

    void Activate()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.simulated = true;
        animator.ActivateAnimator();
        isActive = true;

        Debug.Log($"[Enemy] Activated: {gameObject.name}");
    }

    void DesActivate()
    {
        rb.simulated = false;
        rb.bodyType = RigidbodyType2D.Static;
        animator.DesActivateAnimator();
        isActive = false;

        Debug.Log($"[Enemy] Deactivated: {gameObject.name}");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isAttacking = true;

        movementController.DisableWalkAnimation();
        attackController.Attack();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isAttacking = false;

        attackController.DisableAttackAnimation();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Debug.Log($"Knockback applied - RB: {collision.rigidbody}, PlayerPos: {player.position}, Force: {knockbackForce}");
        KnockbackUtils.ApplyKnockback(collision.rigidbody, player.position, knockbackForce);
    }
}
