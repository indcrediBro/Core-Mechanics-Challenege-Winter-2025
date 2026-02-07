using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float m_maxHealth = 10f;
    [SerializeField] private bool  m_destroyOnDeath = true;

    public HealthOld HealthOld { get; private set; }

    private void Awake()
    {
        HealthOld = new HealthOld(m_maxHealth);
        HealthOld.OnDeath += HandleDeath;
    }

    private void OnDestroy()
    {
        if (HealthOld != null)
            HealthOld.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        if (m_destroyOnDeath)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    // Convenience passthroughs
    public void Damage(float _amount) => HealthOld.Damage(_amount);
    public void Heal(float _amount)   => HealthOld.Heal(_amount);

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