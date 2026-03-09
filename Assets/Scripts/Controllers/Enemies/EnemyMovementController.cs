using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform player;

    public void Move()
    {
        var isPlayerToTheRight = player.position.x > transform.position.x;
        var speed = enemyData.moveSpeed * Time.fixedDeltaTime;

        spriteRenderer.flipX = !isPlayerToTheRight;

        rb.linearVelocityX = !isPlayerToTheRight ? speed : -speed;
    }
}
