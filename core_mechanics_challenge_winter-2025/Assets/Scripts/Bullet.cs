using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_lifeTime;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private HealthComponent m_healthComponent;
    [SerializeField] private float m_damage;
    [SerializeField] private string m_poolTag;

    private void Launch()
    {
        m_rb.linearVelocity = Vector2.zero;
        m_rb.AddForce(transform.up  * m_moveSpeed, ForceMode2D.Impulse);
    }

    public void OnSpawn()
    {
        CancelInvoke(nameof(Destroy));
        Launch();
        Invoke(nameof(Destroy), m_lifeTime);
    }

    public void OnDespawn()
    {
        // Destroy();
    }

    public void SetDamage(float _damage)
    {
        m_damage = _damage;
    }

    public void SetNewMaxHealth(float _maxHealth)
    {
        m_healthComponent.Health.SetMax(_maxHealth);
    }

    private void Destroy()
    {

        ObjectPoolManager.Instance.Despawn(m_poolTag, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out HealthComponent health) && other.gameObject.layer != LayerMask.NameToLayer("Bullet"))
        {
            health.Damage(m_damage);
            ObjectPoolManager.Instance.Spawn("BulletHit",transform.position,Quaternion.identity);
        }

        m_healthComponent.Damage(1);
    }
}
