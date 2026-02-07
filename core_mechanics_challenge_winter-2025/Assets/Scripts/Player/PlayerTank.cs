using System;
using UnityEngine;

public class PlayerTank : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private Transform m_tankBase;
    [SerializeField] private Transform m_cannon;
    [SerializeField] private Transform m_firepoint;
    [SerializeField] private PlayerInputHandler m_input;
    [SerializeField] private WeaponRig m_weaponRig;
    [SerializeField] private TankAnimator m_animator;
    [SerializeField] private string m_bulletKey;
    [SerializeField] private PlayerStats m_stats;

    private TankMovement m_movement;
    private CannonMovement m_rotation;
    private PlayerShoot m_shooting;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        m_movement = new TankMovement(m_input, m_rb, m_tankBase, m_stats.MoveSpeed);
        m_rotation = new CannonMovement(m_cannon, m_input);
        m_animator.Initialize(m_input);
        RebuildWeapon();
    }

    private void OnEnable()
    {
        m_input.Enable();
    }

    private void OnDisable()
    {
        ObjectPoolManager.Instance.SpawnPooledObject("Explosion", transform.position, Quaternion.identity);
        m_input.Disable();
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Paused)
        {
            m_input.Disable();
            return;
        }

        m_input.Enable();

        m_rotation.Rotate();
        m_shooting.AutoFire(m_bulletKey, Time.deltaTime, m_stats);
        m_animator.SetMoveAmount();
        m_animator.Animate();
    }

    private void FixedUpdate()
    {
        m_movement.Move();
    }

    public void RebuildWeapon()
    {
        var firePoints = m_weaponRig.GetActiveFirePoints(m_stats);
        m_shooting = new PlayerShoot(m_stats, firePoints);
    }

    public PlayerStats GetStats()
    {
        return m_stats;
    }
}
