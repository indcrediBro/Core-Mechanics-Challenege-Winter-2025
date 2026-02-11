using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player/Modules/Rotation")]
public class PlayerRotationModule : PlayerModule
{
    public override void Tick(PlayerContext ctx)
    {
        Rotate(ctx);
    }

    public void Rotate(PlayerContext ctx)
    {
        Vector3 lookDir = Vector3.zero;

        if (ctx.Input.m_LastAimDevice is Gamepad)
        {
            Vector2 aim = ctx.Input.m_AimInput;
            if (aim.sqrMagnitude < 0.01f) return;

            lookDir = new Vector3(aim.x, 0f, aim.y);
        }
        else if (ctx.Input.m_LastAimDevice is Mouse)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane plane = new Plane(Vector3.up, ctx.Cannon.position);

            if (!plane.Raycast(ray, out float dist)) return;

            Vector3 hitPoint = ray.GetPoint(dist);
            lookDir = hitPoint - ctx.Cannon.position;
            lookDir.y = 0f;
        }

        if (lookDir.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
        ctx.Cannon.rotation = targetRot;
    }
}