using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerShoot
{
    private readonly List<Transform> m_activeFirePoints;
    private float m_timer;

    public PlayerShoot(PlayerStats _stats, List<Transform> _firePoints)
    {
        m_activeFirePoints = _firePoints;
    }

    public void AutoFire(string _key, float _deltaTime, PlayerStats _stats)
    {
        if(GameManager.Instance.State != GameState.Playing)
            return;

        m_timer -= _deltaTime;
        if (m_timer > 0f) return;

        foreach (var fp in m_activeFirePoints)
        {
            GameObject bullet = ObjectPoolManager.Instance.SpawnPooledObject(
                _key,
                fp.position,
                fp.rotation
            );

            if (bullet.TryGetComponent(out Bullet b))
            {
                b.SetDamage(_stats.Damage);
                b.SetNewMaxHealth(_stats.BulletPierce);
                bullet.transform.localScale = Vector3.one * _stats.BulletSize;
            }
        }

        AudioManager.Instance.PlaySound("SFX_Shoot");
        m_timer = _stats.FireRate;
    }
}