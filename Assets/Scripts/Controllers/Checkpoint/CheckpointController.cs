using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private CheckpointAnimator checkpointAnimator;

    private bool isActivated = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player") || isActivated) return;

        checkpointAnimator.SetActivatedAnimation(true);
        GameManager.Instance.ActivateCheckpoint(gameObject.transform.position);

        isActivated = true;

        Debug.Log("Checkpoint activated");
    }
}
