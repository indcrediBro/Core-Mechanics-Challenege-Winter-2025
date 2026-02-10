using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_lifeTime;
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private BulletHealth m_healthComponent;
    [SerializeField] private int m_damage;
    private float timer;

    private void Launch()
    {
        m_rb.linearVelocity = Vector3.zero;
        m_rb.AddForce(transform.forward  * m_moveSpeed, ForceMode.Impulse);
    }

    public void OnEnable()
    {
        timer = m_lifeTime;
        Launch();
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
        {
            gameObject.SetActive(false);
        }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            return;

        if (other.TryGetComponent(out Health otherHealth))
        {
            otherHealth.TakeDamage(m_damage);
        }

        ObjectPoolManager.Instance.SpawnPooledObject("BulletHit", transform.position, Quaternion.identity).SetActive(true);
        m_healthComponent.TakeDamage(1);

    }
}
