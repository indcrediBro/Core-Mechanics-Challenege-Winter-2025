using UnityEngine;

using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Enemy/Movement/GridMove")]
public class GridMoveModule : EnemyModule
{
    public float speed = 2f;
    public float decisionInterval = 1.2f;
    public float stepDistance = 1f;

    private Vector2[] fallbackDirs =
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    public override void OnEnter(EnemyContext ctx)
    {
        if (ctx.agent == null)
        {
            Debug.LogError($"{name} requires NavMeshAgent", ctx.self);
            return;
        }

        ctx.agent.speed = speed;
        ctx.agent.updateRotation = false;
        ctx.agent.updateUpAxis = false;
        ctx.agent.stoppingDistance = 0f;

        ctx.moveTimer = 0f;
        PickDirection(ctx);
    }

    public override void Tick(EnemyContext ctx)
    {
        if (!ctx.agent.isOnNavMesh)
            return;

        if(RunManager.Instance.freezeEnemies) return;

        ctx.moveTimer -= ctx.deltaTime;

        if (ctx.moveTimer <= 0f || ctx.agent.remainingDistance < 0.05f)
        {
            PickDirection(ctx);
            ctx.moveTimer = decisionInterval;
        }

        if (ctx.agent.velocity.sqrMagnitude > 0.01f)
        {
            ctx.self.rotation =
                Quaternion.LookRotation(ctx.agent.velocity);
        }
    }

    void PickDirection(EnemyContext ctx)
    {
        Vector2 dir2D;

        if (ctx.target != null)
        {
            Vector3 toTarget =
                ctx.target.position - ctx.self.position;

            if (Mathf.Abs(toTarget.x) > Mathf.Abs(toTarget.z))
                dir2D = new Vector2(Mathf.Sign(toTarget.x), 0);
            else
                dir2D = new Vector2(0, Mathf.Sign(toTarget.z));
        }
        else
        {
            dir2D = fallbackDirs[Random.Range(0, fallbackDirs.Length)];
        }

        Vector3 step =
            new Vector3(dir2D.x, 0, dir2D.y) * stepDistance;

        Vector3 targetPos = ctx.self.position + step;

        ctx.agent.SetDestination(targetPos);
        ctx.moveDir = dir2D;

        ctx.self.rotation = Quaternion.LookRotation(step);
    }
}
