using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerInputHandler
{
    public Vector2 m_MoveInput { get; private set; }
    public Vector2 m_AimInput  { get; private set; }
    public bool    ShootPressed { get; private set; }
    public InputDevice m_LastAimDevice { get; private set; }

    [SerializeField] private InputActionReference m_moveAction;
    [SerializeField] private InputActionReference m_aimAction;
    [SerializeField] private InputActionReference m_shootAction;

    public void Enable()
    {
        m_moveAction.action.Enable();
        m_aimAction.action.Enable();
        m_shootAction.action.Enable();

        m_moveAction.action.performed += OnMove;
        m_moveAction.action.canceled  += OnMove;

        m_aimAction.action.performed += OnAim;
        m_aimAction.action.canceled  += OnAim;

        m_shootAction.action.performed += OnShoot;
    }

    public void Disable()
    {
        m_moveAction.action.performed -= OnMove;
        m_moveAction.action.canceled  -= OnMove;

        m_aimAction.action.performed -= OnAim;
        m_aimAction.action.canceled  -= OnAim;

        m_shootAction.action.performed -= OnShoot;

        m_moveAction.action.Disable();
        m_aimAction.action.Disable();
        m_shootAction.action.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.State != GameState.Playing) return;
        m_MoveInput = ctx.ReadValue<Vector2>();
    }

    private void OnAim(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.State != GameState.Playing) return;
        m_AimInput = ctx.ReadValue<Vector2>();
        m_LastAimDevice = ctx.control.device;
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.State != GameState.Playing) return;
        ShootPressed = true;
    }

    public void ConsumeShoot()
    {
        ShootPressed = false;
    }
}