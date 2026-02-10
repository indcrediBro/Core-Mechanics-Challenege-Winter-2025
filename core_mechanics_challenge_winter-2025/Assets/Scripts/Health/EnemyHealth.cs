using System;
using UnityEngine;

public class EnemyHealth : Health
{
    [Space] [SerializeField] private int pointsForKill;

    protected override void OnEnable()
    {
        base.OnEnable();
        OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        Vector3 pos = transform.position;
        ScoreManager.Instance.AddScore(pointsForKill,pos);
        ObjectPoolManager.Instance.SpawnPooledObject("Explosion", transform.position, Quaternion.identity)
            .SetActive(true);
    }
}