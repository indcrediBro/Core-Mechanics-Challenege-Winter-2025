using System;
using UnityEngine;
using Pathfinding;

public class EnemyTank : MonoBehaviour, IPoolable
{
    [Header("References")]
    [SerializeField] private Transform m_cannon;
    [SerializeField] private Transform m_firePoint;
    [SerializeField] private Seeker m_seeker;
    [SerializeField] private HealthComponent m_health;

    [Header("Targets")]
    [SerializeField] private Transform m_player;
    [SerializeField] private Transform m_playerBase;

    [Header("Stats")]
    [SerializeField] private float m_moveSpeed = 2.5f;
    [SerializeField] private float m_rotateSpeed = 360f;
    [SerializeField] private float m_fireRate = 0.6f;
    [SerializeField] private float m_repathRate = 0.5f;
    [SerializeField] private float m_playerAggroRange = 5f;

    [Header("Combat")]
    [SerializeField] private string m_bulletKey;

    private EnemyBrain m_brain;
    private EnemyMovement m_movement;
    private EnemyCannon m_cannonLogic;
    private EnemyShooter m_shooter;

    private float m_repathTimer;

    private void Start()
    {
        m_brain = new EnemyBrain(transform, m_player, m_playerBase, m_playerAggroRange);
        m_movement = new EnemyMovement(transform, m_seeker, m_moveSpeed, 0.2f);
        m_cannonLogic = new EnemyCannon(m_cannon, m_rotateSpeed);
        m_shooter = new EnemyShooter(m_firePoint, m_fireRate);

        m_health.Health.OnDeath += OnDeath;
    }

    public void Initialize()
    {
        m_player = GameManager.Instance.GetPlayer().transform;
        m_playerBase = GameManager.Instance.GetPlayerBase().transform;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        Transform target = m_brain.GetTarget();
        if (target == null)
            return;

        // Pathing
        m_repathTimer -= dt;
        if (m_repathTimer <= 0f)
        {
            m_movement.RequestPath(target.position);
            m_repathTimer = m_repathRate;
        }

        m_movement.Tick(dt);

        // Combat
        m_cannonLogic.AimAt(target.position, dt);
       m_shooter.Tick(m_bulletKey, 1, dt);
    }

    private void OnDeath()
    {
        ObjectPoolManager.Instance.Spawn("Explosion",transform.position,Quaternion.identity);
        ObjectPoolManager.Instance.Despawn("BaseEnemy",gameObject);
    }

    public void OnSpawn()
    {
        m_health.ResetHealth();
        Initialize();
    }

    public void OnDespawn()
    {
    }
}