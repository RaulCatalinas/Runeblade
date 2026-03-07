using UnityEngine;

public class PlatformActivateTriggerController : MonoBehaviour
{
    [SerializeField] private PlatformController platform;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        platform.ActivatePlatform();
    }
}
