using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void ActivateAnimator()
    {
        animator.enabled = true;
    }

    public void DesActivateAnimator()
    {
        animator.enabled = false;
    }

    public void SetWalkAnimation(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void SetAttackAnimation(bool isAttacking)
    {
        animator.SetBool("isAttacking", isAttacking);
    }
}
