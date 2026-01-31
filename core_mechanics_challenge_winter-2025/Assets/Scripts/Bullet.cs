using System;
using DefaultNamespace;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_lifeTime;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private HealthComponent m_healthComponent;

    private void OnEnable()
    {
        Destroy(gameObject, m_lifeTime);
    }


    private void Launch()
    {
        m_rb.AddForce(transform.up  * m_moveSpeed, ForceMode2D.Impulse);
    }

    public void OnSpawn()
    {
        Launch();
        Invoke(nameof(Destroy), m_lifeTime);
    }

    public void OnDespawn()
    {
        // Destroy();
    }

    private void Destroy()
    {
        ObjectPoolManager.Instance.Despawn("Bullet", gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out HealthComponent health))
        {
            health.Damage(1);
        }

        m_healthComponent.Damage(1);
    }
}
