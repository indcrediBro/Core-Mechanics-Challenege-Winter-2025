using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_lifeTime;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private BulletHealth m_healthComponent;
    [SerializeField] private int m_damage;
    private float timer;

    private void Launch()
    {
        m_rb.linearVelocity = Vector2.zero;
        m_rb.AddForce(transform.up  * m_moveSpeed, ForceMode2D.Impulse);
    }

    public void OnEnable()
    {
        timer = m_lifeTime;
        Launch();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetDamage(int _damage)
    {
        m_damage = _damage;
    }

    public void SetNewMaxHealth(int _maxHealth)
    {
        m_healthComponent.SetMaxHealth(_maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            return;

        if (other.TryGetComponent(out Health otherHealth))
        {
            otherHealth.TakeDamage(m_damage);
            ObjectPoolManager.Instance.SpawnPooledObject("BulletHit", transform.position, Quaternion.identity);
        }

        m_healthComponent.TakeDamage(1);
    }
}
