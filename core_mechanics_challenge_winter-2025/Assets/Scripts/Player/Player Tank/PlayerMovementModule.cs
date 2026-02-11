using UnityEngine;
using UnityEngine.InputSystem.Editor;

[CreateAssetMenu(menuName = "Player/Modules/Movement")]
public class PlayerMovementModule : PlayerModule
{
    public override void FixedTick(PlayerContext ctx)
    {

        Vector2 input = ctx.Input.m_MoveInput;
        if (GameManager.Instance.State != GameState.Playing)
        {
            input = Vector2.zero;
        }

        Vector3 dir = new Vector3(input.x, 0f, input.y);

        ctx.Rigidbody.MovePosition(
            ctx.Rigidbody.position +
            dir * ctx.Stats.MoveSpeed * Time.fixedDeltaTime
        );

        if (dir.sqrMagnitude > 0.01f)
            ctx.TankBase.forward = dir;

        Rotate(ctx, input);
    }

    private void Rotate(PlayerContext ctx, Vector2 input)
    {
        if (input.sqrMagnitude < 0.01f)
            return;

        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        ctx.TankBase.rotation = Quaternion.Euler(0f, angle,0f);
    }
}