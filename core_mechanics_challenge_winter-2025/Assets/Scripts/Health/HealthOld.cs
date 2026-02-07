using System;
using UnityEngine;

public class HealthOld
{
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    public float Current { get; private set; }
    public float Max { get; private set; }

    public bool IsDead => Current <= 0f;

    public HealthOld(float maxHealth)
    {
        Max = Mathf.Max(1f, maxHealth);
        Current = Max;
    }

    public void Damage(float amount)
    {
        if (IsDead || amount <= 0f)
            return;

        Current = Mathf.Clamp(Current - amount, 0f, Max);
        OnHealthChanged?.Invoke(Current, Max);

        if (Current <= 0f)
            OnDeath?.Invoke();
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f)
            return;

        Current = Mathf.Clamp(Current + amount, 0f, Max);
        OnHealthChanged?.Invoke(Current, Max);
    }

    public void SetMax(float newMax, bool healToFull = false)
    {
        Max = Mathf.Max(1f, newMax);
        if (healToFull)
            Current = Max;

        Current = Mathf.Clamp(Current, 0f, Max);
        OnHealthChanged?.Invoke(Current, Max);
    }
}