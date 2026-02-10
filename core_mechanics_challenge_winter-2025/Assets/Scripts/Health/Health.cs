using System;
using System.Collections;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public event Action OnDamaged;
    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    [Header("Health Settings")]
    [Space(2)]
    [SerializeField] protected bool m_dontDestroy;

    [SerializeField] protected int m_maxHealth;
    [SerializeField] protected int m_currentHealth;
    [SerializeField] protected float m_waitTimeBeforeDeath = 0f;

    protected bool m_isDead;

    public virtual int GetMaxHealthValue() { return m_maxHealth; }
    public virtual int GetCurrentHealthValue() { return m_currentHealth; }
    public virtual bool IsDead() { return m_isDead; }

    public virtual void Heal(int _value)
    {
        ChangeHealth(m_currentHealth + _value);
    }

    public virtual void TakeDamage(int _damage)
    {
        ChangeHealth(m_currentHealth - _damage);
        OnDamaged?.Invoke();

        if (m_currentHealth <= 0)
        {
            Die(m_waitTimeBeforeDeath);
        }
    }

    protected virtual void Die(float _timeBeforeRemoving)
    {
        StartCoroutine(DieCO(_timeBeforeRemoving));
    }

    public void ResetHealthToMax()
    {
        ChangeHealth(m_maxHealth);
        m_isDead = false;
    }

    public void SetMaxHealth(int _amount)
    {
        m_maxHealth = _amount;
        ResetHealthToMax();
    }

    protected virtual void OnEnable()
    {
        ResetHealthToMax();
    }

    private void ChangeHealth(int _value)
    {
        m_currentHealth = Mathf.Clamp(_value, 0, m_maxHealth);
        OnHealthChanged?.Invoke(m_currentHealth, m_maxHealth);
    }

    private void Deactivate()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private IEnumerator DieCO(float _timeToWait)
    {

        m_isDead = true;
        OnDeath?.Invoke();
        yield return new WaitForSeconds(_timeToWait);
        if (m_dontDestroy)
            Deactivate();
        else
            Destroy();
    }
}
