using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerInputHandler
{
    [SerializeField] private InputActionReference m_moveInputAction;
    [SerializeField] private InputActionReference m_aimInputAction;

    public Vector2 GetMovement()
    {
        return m_moveInputAction.action.ReadValue<Vector2>();
    }

    public Vector2 GetAimDirection(Vector3 origin)
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorld.z = 0f;

        return (mouseWorld - origin);
    }
}
