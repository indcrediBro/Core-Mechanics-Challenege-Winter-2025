using System;
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
        else
            gameObject.SetActive(false);
    }

    // Convenience passthroughs
    public void Damage(float _amount) => Health.Damage(_amount);
    public void Heal(float _amount)   => Health.Heal(_amount);

    public void ResetHealth()
    {
        Heal(m_maxHealth);
    }

    public void SetMaxHealth(float _maxHealth)
    {
        m_maxHealth = _maxHealth;
        Heal(m_maxHealth);
    }
}