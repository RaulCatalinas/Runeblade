using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private CheckpointAnimator checkpointAnimator;
    [SerializeField] private Collider2D checkpointCollider;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        checkpointCollider.enabled = false;

        checkpointAnimator.SetActivatedAnimation(true);
        GameManager.Instance.ActivateCheckpoint(gameObject.transform.position);

        Debug.Log("Checkpoint activated");
    }
}
