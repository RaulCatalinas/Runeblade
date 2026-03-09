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

    private float activationRangeSqr;
    private float desActivationRangeSqr;

    void Awake()
    {
        activationRangeSqr = Mathf.Pow(activationRange, 2);
        desActivationRangeSqr = Mathf.Pow(desActivationRange, 2);
        circleCollider.radius = enemyData.detectionRange;
    }

    void FixedUpdate()
    {
        var sqrDistance = (transform.position - player.position).sqrMagnitude;

        if (!isActive && sqrDistance <= activationRangeSqr)
        {
            Activate();
        }
        else if (isActive && sqrDistance >= desActivationRangeSqr)
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
