using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyAnimator enemyAnimator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform player;

    public void Move()
    {
        enemyAnimator.SetWalkingAnimation(true);

        var isPlayerToTheRight = player.position.x > transform.position.x;

        spriteRenderer.flipX = !isPlayerToTheRight;
        rb.linearVelocityX = isPlayerToTheRight ? enemyData.moveSpeed : -enemyData.moveSpeed;
    }
}
