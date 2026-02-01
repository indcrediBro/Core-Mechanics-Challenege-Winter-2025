using DefaultNamespace;
using UnityEngine;

public class EnemyShooter
{
    private readonly Transform m_firePoint;
    private readonly float m_fireRate;
    private float m_timer;

    public EnemyShooter(Transform firePoint, float fireRate)
    {
        m_firePoint = firePoint;
        m_fireRate = fireRate;
        m_timer = 0f;
    }

    public void Tick(string bulletKey, float deltaTime)
    {
        m_timer -= deltaTime;
        if (m_timer > 0f)
            return;

        ObjectPoolManager.Instance.Spawn(bulletKey, m_firePoint.position, m_firePoint.rotation);
        m_timer = m_fireRate;
    }
}