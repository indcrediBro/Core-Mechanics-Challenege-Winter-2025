using UnityEngine;
using UnityEngine.InputSystem;

public class CannonMovement
{
    private readonly Transform m_cannon;
    private readonly PlayerInputHandler m_input;
    private readonly Camera m_cam;

    public CannonMovement(Transform cannon, PlayerInputHandler input)
    {
        m_cannon = cannon;
        m_input = input;
        m_cam = Camera.main;
    }

    public void Rotate()
    {
        Vector3 lookDir = Vector3.zero;

        if (m_input.m_LastAimDevice is Gamepad)
        {
            Vector2 aim = m_input.m_AimInput;
            if (aim.sqrMagnitude < 0.01f) return;

            lookDir = new Vector3(aim.x, 0f, aim.y);
        }
        else if (m_input.m_LastAimDevice is Mouse)
        {
            Ray ray = m_cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane plane = new Plane(Vector3.up, m_cannon.position);

            if (!plane.Raycast(ray, out float dist)) return;

            Vector3 hitPoint = ray.GetPoint(dist);
            lookDir = hitPoint - m_cannon.position;
            lookDir.y = 0f;
        }

        if (lookDir.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
        m_cannon.rotation = targetRot;
    }
}