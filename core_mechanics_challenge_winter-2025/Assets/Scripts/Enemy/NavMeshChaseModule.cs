using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Enemy/Movement/NavMeshChase")]
public class NavMeshChaseModule : EnemyModule
{
    public float stopDistance = 5f;
    public float agroDistance = 5f;
    public override void OnEnter(EnemyContext ctx)
    {
        ctx.agent.stoppingDistance = stopDistance;
        ctx.agent.updateRotation = false;
        ctx.agent.updateUpAxis = false;
    }

    public override void Tick(EnemyContext ctx)
    {
        if (!ctx.agent.isOnNavMesh) return;

        ctx.agent.SetDestination(ctx.target.position);

        Vector3 v = ctx.agent.velocity;
        if (v.sqrMagnitude > 0.1f)
            ctx.self.rotation = Quaternion.LookRotation(v);
    }
}