using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyAnimator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform player;

    public void Move()
    {
        animator.SetWalkAnimation(true);

        var isPlayerToTheRight = player.position.x > transform.position.x;

        spriteRenderer.flipX = !isPlayerToTheRight;
        rb.linearVelocityX = isPlayerToTheRight ? enemyData.moveSpeed : -enemyData.moveSpeed;
    }

    public void DisableWalkAnimation()
    {
        animator.SetWalkAnimation(true);
    }
}
