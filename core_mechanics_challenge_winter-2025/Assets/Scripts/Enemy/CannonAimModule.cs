using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Combat/CannonAim")]
public class CannonAimModule : EnemyModule
{
    public float rotateSpeed = 360f;

    public override void Tick(EnemyContext ctx)
    {
        if(RunManager.Instance.freezeEnemies) return;

        Transform target = ctx.player != null ? ctx.player : ctx.playerBase;
        if (target == null) return;

        Vector3 dir = target.position - ctx.cannon.position;
        dir.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        ctx.cannon.rotation = Quaternion.RotateTowards(
            ctx.cannon.rotation,
            targetRot,
            rotateSpeed * ctx.deltaTime
        );
    }
}