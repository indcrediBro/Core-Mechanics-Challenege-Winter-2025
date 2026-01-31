using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float m_maxHealth = 10f;
    [SerializeField] private bool  m_destroyOnDeath = true;

    public Health Health { get; private set; }

    private void Awake()
    {
        Health = new Health(m_maxHealth);
        Health.OnDeath += HandleDeath;
    }

    private void OnDestroy()
    {
        if (Health != null)
            Health.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        if (m_destroyOnDeath)
            Destroy(gameObject);
    }

    // Convenience passthroughs
    public void Damage(float _amount) => Health.Damage(_amount);
    public void Heal(float _amount)   => Health.Heal(_amount);
}