using UnityEngine;

public static class LineOfSight
{
    public static bool HasLOS(
        Vector2 origin,
        Vector2 target,
        LayerMask obstacleMask
    )
    {
        Vector2 dir = target - origin;
        float dist = dir.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            dir.normalized,
            dist,
            obstacleMask
        );

        return hit.collider == null;
    }
}