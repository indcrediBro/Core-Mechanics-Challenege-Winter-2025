using System;
using UnityEngine;
using Pathfinding;

public class EnemyTank : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform m_cannon;
    [SerializeField] private Transform m_firePoint;
    [SerializeField] private Seeker m_seeker;
    [SerializeField] private EnemyHealth m_health;

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

    }

    public void OnEnable()
    {
        m_health.OnDeath += OnDeath;
        m_player = GameManager.Instance.GetPlayer().transform;
        m_playerBase = GameManager.Instance.GetPlayerBase().transform;
    }

    private void OnDisable()
    {
        m_health.OnDeath -= OnDeath;
    }

    private void Update()
    {
        Transform target = m_brain.GetTarget();
        if (target == null) return;

        HandlePathFinding(target,Time.deltaTime);
        HandleMovement(Time.deltaTime);
        HandleCombat(target, Time.deltaTime);
    }

    private void HandlePathFinding(Transform target, float dt)
    {
        m_repathTimer -= dt;
        if (m_repathTimer <= 0f)
        {
            m_movement.RequestPath(target.position);
            m_repathTimer = m_repathRate;
        }
    }

    private void HandleMovement(float _dt)
    {
        m_movement.Tick(_dt);
    }

    private void HandleCombat(Transform _target, float _dt)
    {
        m_cannonLogic.AimAt(_target.position, _dt);
        m_shooter.Tick(m_bulletKey, 1, _dt);
    }

    private void OnDeath()
    {
        GameObject explosionVFX = ObjectPoolManager.Instance.GetPooledObject("Explosion");
        explosionVFX.transform.position = transform.position;
        explosionVFX.SetActive(true);
    }
}