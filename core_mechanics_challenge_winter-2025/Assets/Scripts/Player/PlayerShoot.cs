using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerShoot
{
    private readonly List<Transform> m_activeFirePoints;
    private float m_timer;

    public PlayerShoot(PlayerStats stats, List<Transform> firePoints)
    {
        m_activeFirePoints = firePoints;
        m_timer = 0f;
    }

    public void TickCooldown(float dt)
    {
        m_timer -= dt;
    }

    public void TryShoot(
        string bulletKey,
        PlayerStats stats,
        PlayerInputHandler input
    )
    {
        if (m_timer > 0f)
            return;

        if (!input.ShootPressed)
            return;

        foreach (var fp in m_activeFirePoints)
        {
            GameObject bullet = ObjectPoolManager.Instance.SpawnPooledObject(
                bulletKey,
                fp.position,
                fp.rotation
            );

            if (bullet.TryGetComponent(out Bullet b))
            {
                b.SetDamage(stats.Damage);
                b.SetNewMaxHealth(stats.BulletPierce);
                bullet.transform.localScale =
                    Vector3.one * stats.BulletSize;
            }

            bullet.SetActive(true);
        }

        AudioManager.Instance.PlaySound("SFX_Shoot");
        GameManager.Instance.ShotFired();
        m_timer = stats.FireRate;
        input.ConsumeShoot();
    }
}