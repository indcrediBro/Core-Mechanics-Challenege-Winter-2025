using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerShoot
{
    private readonly PlayerStats m_stats;
    private readonly List<Transform> m_activeFirePoints;
    private float m_timer;

    public PlayerShoot(PlayerStats stats, List<Transform> firePoints)
    {
        m_stats = stats;
        m_activeFirePoints = firePoints;
    }

    public void AutoFire(string key, float deltaTime)
    {
        m_timer -= deltaTime;
        if (m_timer > 0f) return;

        foreach (var fp in m_activeFirePoints)
        {
            GameObject bullet = ObjectPoolManager.Instance.Spawn(
                key,
                fp.position,
                fp.rotation
            );

            if (bullet.TryGetComponent(out Bullet b))
            {
                b.SetDamage(m_stats.Damage);
                b.SetNewMaxHealth(m_stats.BulletPierce);
                bullet.transform.localScale = Vector3.one * m_stats.BulletSize;
            }
        }

        // AudioManager.Instance.PlaySound("SFX_Shoot");
        m_timer = m_stats.FireRate;
    }
}