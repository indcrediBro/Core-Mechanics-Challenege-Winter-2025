using System;
using DefaultNamespace;
using UnityEngine;

[Serializable]
public class PlayerShoot
{
    private readonly Transform m_firePoint;
    private readonly float m_fireRate;

    private float m_timer;

    public PlayerShoot(Transform _firePoint, float _fireRate)
    {
        m_firePoint = _firePoint;
        m_fireRate = _fireRate;
    }

    public void AutoFire(string _key, float _deltaTime)
    {
        m_timer -= _deltaTime;

        if (m_timer <= 0f)
        {
            ObjectPoolManager.Instance.Spawn(
                _key,
                m_firePoint.position,
                m_firePoint.rotation
            );

            m_timer = m_fireRate;
        }
    }
}
