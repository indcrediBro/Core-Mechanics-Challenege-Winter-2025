using UnityEngine;

public class PlayerTank : MonoBehaviour
{
    [SerializeField] private Rigidbody2D  m_rb;
    [SerializeField] private Transform    m_cannon;
    [SerializeField] private Transform    m_firepoint;
    [SerializeField] private PlayerInputHandler m_input;
    [SerializeField] private TankAnimator m_animator;
    [SerializeField] private string m_bulletKey;

    [Header("Stats")]
    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private float m_rotateSpeed = 720f;
    [SerializeField] private float m_fireRate = 0.1f;

    private TankMovement m_movement;
    private CannonMovement m_rotation;
    private PlayerShoot m_shooting;

    private Vector2 m_moveInput;
    private Vector2 m_aimInput;

    private void Awake()
    {
        m_movement = new TankMovement(m_rb, m_moveSpeed);
        m_rotation = new CannonMovement(m_cannon, m_rotateSpeed);
        m_shooting = new PlayerShoot(m_firepoint, m_fireRate);
    }

    private void Update()
    {
        m_moveInput = m_input.GetMovement();
        m_aimInput  = m_input.GetAimDirection(m_cannon.position);

        m_rotation.Rotate(m_aimInput, Time.deltaTime);
        m_shooting.AutoFire(m_bulletKey, Time.deltaTime);
        m_animator.SetMoveAmount(m_moveInput.sqrMagnitude);
        m_animator.Animate();
    }

    private void FixedUpdate()
    {
        m_movement.Move(m_moveInput);
    }
}
