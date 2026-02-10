using UnityEngine;

public class SimpleAnimState
{
    public float timer;
    public int frameIndex;
    public bool isMoving;
}

[CreateAssetMenu(menuName = "Enemy/Visuals/SimpleSpriteAnimation")]
public class SimpleSpriteAnimationModule : EnemyModule
{
    [Header("Animation")]
    public Sprite[] frames;
    public float frameTime = 0.1f;
    public float moveThreshold = 0.05f;

    public override void OnEnter(EnemyContext ctx)
    {
        ctx.animState.timer = 0f;
        ctx.animState.frameIndex = 0;
    }

    public override void Tick(EnemyContext ctx)
    {
        if (ctx.spriteRenderer == null || frames.Length == 0)
            return;

        Vector3 velocity = ctx.agent != null ? ctx.agent.velocity : Vector3.zero;
        ctx.animState.isMoving = velocity.sqrMagnitude > moveThreshold * moveThreshold;

        if (!ctx.animState.isMoving)
        {
            ctx.spriteRenderer.sprite =
                frames[0];
            return;
        }

        ctx.animState.timer -= ctx.deltaTime;
        if (ctx.animState.timer > 0f)
        {
            return;
        }

        ctx.animState.frameIndex =
            (ctx.animState.frameIndex + 1) % frames.Length;

        ctx.spriteRenderer.sprite =
            frames[ctx.animState.frameIndex];

        ctx.animState.timer = frameTime;
    }
}

