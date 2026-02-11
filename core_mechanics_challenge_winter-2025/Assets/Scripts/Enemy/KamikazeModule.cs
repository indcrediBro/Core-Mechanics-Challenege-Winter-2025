using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Enemy/Behaviours/Kamikaze")]
public class KamikazeModule : EnemyModule
{
    [Header("Chase")]
    public float chaseSpeedMultiplier = 1.5f;
    public float detonationRange = 1.2f;
    public float stoppingDistance = 0.25f;
    [Header("Explosion")]
    public float explosionRadius = 2.5f;
    public int damage = 5;
    public LayerMask damageMask;

    [Header("VFX")]
    public string explosionVFXKey = "Explosion";

    public override void OnEnter(EnemyContext ctx)
    {
        if (ctx.agent != null)
        {
            ctx.agent.speed *= chaseSpeedMultiplier;
            ctx.agent.stoppingDistance = stoppingDistance;
            ctx.agent.updateRotation = false;
            ctx.agent.angularSpeed = 0f;
        }
    }

    public override void Tick(EnemyContext ctx)
    {
        Transform target = ctx.player != null ? ctx.player : ctx.playerBase;
        if (target == null || ctx.agent == null)
            return;

        ctx.agent.SetDestination(target.position);

        float dist = Vector3.Distance(ctx.self.position, target.position);
        if (dist <= detonationRange)
        {
            Explode(ctx);
        }
    }

    private void Explode(EnemyContext ctx)
    {
        // VFX
        if (!string.IsNullOrEmpty(explosionVFXKey))
        {
            GameObject vfx = ObjectPoolManager.Instance.SpawnPooledObject(
                explosionVFXKey,
                ctx.self.position,
                Quaternion.identity
            );
            vfx.SetActive(true);
        }

        // Damage
        Collider[] hits = Physics.OverlapSphere(
            ctx.self.position,
            explosionRadius,
            damageMask
        );

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }
        }

        // Kill self
        ctx.health.TakeDamage(damage * 10);
    }
}