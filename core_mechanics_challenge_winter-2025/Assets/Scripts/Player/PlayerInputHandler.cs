using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerInputHandler
{
    public Vector2 m_MoveInput { get; private set; }
    public Vector2 m_AimInput { get; private set; }
    public InputDevice m_LastAimDevice { get; private set; }

    [SerializeField] private InputActionReference m_moveAction;
    [SerializeField] private InputActionReference m_aimAction;

    public void Enable()
    {
        m_moveAction.action.Enable();
        m_aimAction.action.Enable();

        m_moveAction.action.performed += OnMove;
        m_moveAction.action.canceled += OnMove;

        m_aimAction.action.performed += OnAim;
        m_aimAction.action.canceled += OnAim;
    }

    public void Disable()
    {
        m_moveAction.action.performed -= OnMove;
        m_moveAction.action.canceled -= OnMove;

        m_aimAction.action.performed -= OnAim;
        m_aimAction.action.canceled -= OnAim;

        m_moveAction.action.Disable();
        m_aimAction.action.Disable();
    }

    private void OnMove(InputAction.CallbackContext _context)
    {
        m_MoveInput = _context.ReadValue<Vector2>();
    }

    private void OnAim(InputAction.CallbackContext _context)
    {
        m_AimInput = _context.ReadValue<Vector2>();
        m_LastAimDevice = _context.control.device;

    }
}