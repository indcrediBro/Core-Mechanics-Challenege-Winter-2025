using UnityEngine;

public class EnemyShooter
{
    private readonly Transform m_firePoint;
    private readonly float m_fireRate;
    private float m_timer;

    public EnemyShooter(Transform _firePoint, float _fireRate)
    {
        m_firePoint = _firePoint;
        m_fireRate = _fireRate;
        m_timer = 0f;
    }

    public void Tick(string _bulletKey, int _damage, float _deltaTime)
    {
        m_timer -= _deltaTime;
        if (m_timer > 0f)
            return;

        GameObject bullet = ObjectPoolManager.Instance.SpawnPooledObject(_bulletKey, m_firePoint.position, m_firePoint.rotation);
        bullet.TryGetComponent(out Bullet bulletComponent);
        if (bulletComponent)
        {
            bulletComponent.SetNewMaxHealth(1);
            bulletComponent.SetDamage(_damage);
        }
        m_timer = m_fireRate;
    }
}