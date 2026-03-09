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

    public void SetWalkingAnimation(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }
}
