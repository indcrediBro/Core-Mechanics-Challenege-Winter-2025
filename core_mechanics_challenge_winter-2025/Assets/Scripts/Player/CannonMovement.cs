using UnityEngine;
using UnityEngine.InputSystem;

public class CannonMovement
{
    private readonly Transform m_target;
    private readonly Rigidbody2D m_rb;
    private readonly PlayerInputHandler _mInputHandler;

    public CannonMovement(Transform _target, PlayerInputHandler _inputHandler)
    {
        m_target = _target;
        _mInputHandler = _inputHandler;
    }

    public void Rotate()
    {
        Vector2 lookDir = Vector2.zero;

        if (_mInputHandler.m_LastAimDevice is Gamepad)
        {
            lookDir = _mInputHandler.m_AimInput.normalized;
        }
        else if (_mInputHandler.m_LastAimDevice is Mouse)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = m_target.position.z;
            lookDir = (mousePos - m_target.position).normalized;
        }

        if (lookDir.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
            m_target.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
