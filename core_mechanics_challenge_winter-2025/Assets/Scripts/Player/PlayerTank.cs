using System;
using UnityEngine;

public class PlayerTank : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private Transform m_tankBase;
    [SerializeField] private Transform m_cannon;
    [SerializeField] private Transform m_firepoint;
    [SerializeField] private PlayerInputHandler m_input;
    [SerializeField] private WeaponRig m_weaponRig;
    [SerializeField] private TankAnimator m_animator;
    [SerializeField] private string m_bulletKey;
    [SerializeField] private PlayerStats m_stats;
    [SerializeField] private PlayerHealth m_health;

    private TankMovement m_movement;
    private CannonMovement m_rotation;
    private PlayerShoot m_shooting;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        UpdateLives();
    }

    private void OnEnable()
    {
        m_input.Enable();
        m_health.OnDeath += HandleDeath;
        m_health.OnDamaged += UpdateLives;
    }

    private void OnDisable()
    {
        m_input.Disable();
        m_health.OnDeath -= HandleDeath;
        m_health.OnDamaged -= UpdateLives;
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Paused)
            return;

        m_rotation.Rotate();

        m_shooting.TickCooldown(Time.deltaTime);
        m_shooting.TryShoot(m_bulletKey, m_stats, m_input);

        m_animator.SetMoveAmount();
        m_animator.Animate();
    }

    private void FixedUpdate()
    {
        m_movement.Move();
    }

    private void Initialize()
    {
        m_movement = new TankMovement(m_input, m_rb, m_tankBase, m_stats.MoveSpeed);
        m_rotation = new CannonMovement(m_cannon, m_input);
        m_animator.Initialize(m_input);
        RebuildWeapon();
    }

    public PlayerStats GetStats()
    {
        return m_stats;
    }

    public void RebuildWeapon()
    {
        var firePoints = m_weaponRig.GetActiveFirePoints(m_stats);
        m_shooting = new PlayerShoot(m_stats, firePoints);
    }

    private void HandleDeath()
    {
        AudioManager.Instance.StopSound("SFX_Move");
        GameObject explosion = ObjectPoolManager.Instance.SpawnPooledObject("Explosion", transform.position, Quaternion.identity);
        explosion.SetActive(true);
    }

    private void UpdateLives()
    {
        UIManager.Instance.UpdateLivesUI();
    }
}
