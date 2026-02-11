using UnityEngine;

public abstract class BossBase : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float sightDistance = 10f;
    [SerializeField] protected LayerMask obstacleMask;
    [SerializeField] protected EnemyHealth health;

    protected Transform player;

    protected Vector2[] fallbackDirs =
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    protected virtual void OnEnable()
    {
        player = GameManager.Instance.GetPlayer().transform;
    }

    // -------- LINE OF SIGHT (TOP-DOWN) --------
    protected bool HasLineOfSight()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        Vector3 target = player.position + Vector3.up * 0.1f;
        Vector3 dir = target - origin;

        if (dir.magnitude > sightDistance)
            return false;

        if (Physics.Raycast(origin, dir.normalized, dir.magnitude, obstacleMask))
            return false;

        return true;
    }

    // -------- MOVEMENT --------
    protected void MoveTowards(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        MoveWithAvoidance(dir);
    }

    protected void MoveAway(Vector3 target)
    {
        Vector3 dir = transform.position - target;
        MoveWithAvoidance(dir);
    }

    protected void MoveWithAvoidance(Vector3 desiredDir)
    {
        desiredDir.y = 0f;

        if (desiredDir.sqrMagnitude < 0.001f)
            return;

        desiredDir.Normalize();

        float checkDist = 0.6f;

        // Preferred direction
        if (!Physics.Raycast(transform.position, desiredDir, checkDist, obstacleMask))
        {
            ApplyMovement(desiredDir);
            return;
        }

        // Try fallback directions
        foreach (Vector2 fallback in fallbackDirs)
        {
            Vector3 altDir = new Vector3(fallback.x, 0f, fallback.y);

            if (!Physics.Raycast(transform.position, altDir, checkDist, obstacleMask))
            {
                ApplyMovement(altDir);
                return;
            }
        }

        // Nudge backwards if fully stuck
        ApplyMovement(-desiredDir * 0.5f);
    }

    // -------- ACTUAL MOVE + ROTATION --------
    private void ApplyMovement(Vector3 dir)
    {
        transform.position += dir * moveSpeed * Time.deltaTime;

        // Rotate ONLY around Y, based on movement direction
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
        }
    }
}
