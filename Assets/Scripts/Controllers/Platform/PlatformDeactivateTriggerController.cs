using UnityEngine;

public class PlatformDeactivateTriggerController : MonoBehaviour
{
    [SerializeField] private PlatformController platform;

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        platform.DeactivatePlatform();
    }
}
