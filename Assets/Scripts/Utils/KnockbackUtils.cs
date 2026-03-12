using UnityEngine;

public static class KnockbackUtils
{
    public static void ApplyKnockback(Rigidbody2D rb, Vector2 sourcePosition, float force)
    {
        var direction = (rb.position - sourcePosition).normalized;

        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
