using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Targeting/SimpleAggro")]
public class AggroTargetModule : EnemyModule
{
    public float playerRange = 6f;

    public override void Tick(EnemyContext ctx)
    {
        if (ctx.player == null)
            return;

        float d = Vector3.Distance(ctx.self.position, ctx.player.position);
        ctx.self.LookAt(d <= playerRange ? ctx.player : ctx.playerBase);
        if (d < playerRange)
        {
            ctx.target = ctx.player.transform;
        }
        else
        {
            ctx.target = ctx.playerBase.transform;
        }
    }
}