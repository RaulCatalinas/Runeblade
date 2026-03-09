using UnityEngine;

public static class RangeDetector
{
    public static bool IsInRange(Vector2 position, Vector2 target, float range)
    {
        return (position - target).sqrMagnitude <= range * range;
    }

    public static bool IsOutOfRange(Vector2 position, Vector2 target, float range)
    {
        return (position - target).sqrMagnitude >= range * range;
    }
}
