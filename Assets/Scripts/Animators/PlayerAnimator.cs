using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetMoveAnimation(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }

    public void SetAttackAnimation(bool isAttacking)
    {
        animator.SetBool("isAttacking", isAttacking);
    }

    public void SetDashAnimation(bool isDashing)
    {
        animator.SetBool("isDashing", isDashing);
    }

    public void SetJumpAnimation(bool isJumping)
    {
        animator.SetBool("isJumping", isJumping);
    }

    public void SetFallingAnimation(bool isFalling)
    {
        animator.SetBool("isFalling", isFalling);
    }
}
