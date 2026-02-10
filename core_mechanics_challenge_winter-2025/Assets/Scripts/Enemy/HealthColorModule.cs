using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Modules/Health Color Feedback")]
public class HealthColorModule : EnemyModule
{
    public Color fullHealthColor = Color.white;
    public Color lowHealthColor = Color.red;

    public bool flashOnDamage = true;
    public Color flashColor = Color.white;
    public float flashDuration = 0.08f;

    public override void OnEnter(EnemyContext ctx)
    {
        base.OnEnter(ctx);
        fullHealthColor = ctx.spriteRenderer.color;
        ctx.spriteRenderer.color = new Color(fullHealthColor.r, fullHealthColor.g, fullHealthColor.b, 1);
    }

    public override void Tick(EnemyContext ctx)
    {
        if (ctx.health == null || ctx.spriteRenderer == null)
            return;

        if (ctx.controller.IsFlashing)
            return;

        float t = 1f - (float)ctx.health.GetCurrentHealthValue()
            / ctx.health.GetMaxHealthValue();

        ctx.spriteRenderer.color = Color.Lerp(
            fullHealthColor,
            lowHealthColor,
            t
        );
    }

    public override void OnDamage(EnemyContext ctx)
    {
        if (!flashOnDamage)
            return;

        ctx.controller.Flash(flashColor, flashDuration);
    }
}
