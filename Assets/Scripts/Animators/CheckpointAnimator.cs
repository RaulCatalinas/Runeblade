using UnityEngine;

public class CheckpointAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetActivatedAnimation(bool isActivated)
    {
        animator.SetBool("isActivated", isActivated);
    }
}
