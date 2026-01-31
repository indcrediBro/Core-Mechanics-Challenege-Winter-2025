using System;
using DefaultNamespace;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_lifeTime;
    [SerializeField] private Rigidbody2D m_rb;
    
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
}
