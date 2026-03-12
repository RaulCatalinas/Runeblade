using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private EnemyAnimator animator;

    public void Attack()
    {
        animator.SetAttackAnimation(true);

        rb.linearVelocityX = enemyData.attackSpeed;
    }

    public void DisableAttackAnimation()
    {
        animator.SetAttackAnimation(false);
    }
}
