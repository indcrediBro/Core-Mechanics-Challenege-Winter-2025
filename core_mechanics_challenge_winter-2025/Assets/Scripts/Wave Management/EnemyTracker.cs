using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker
{
    private HashSet<GameObject> alive = new();

    public int ActiveCount => alive.Count;
    public bool HasBoss { get; private set; }

    public IEnumerable<GameObject> All => alive;

    public void RegisterEnemy(GameObject enemy)
    {
        alive.Add(enemy);

        if (enemy.CompareTag("Boss"))
            HasBoss = true;
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        alive.Remove(enemy);

        if (enemy.CompareTag("Boss"))
            HasBoss = false;
    }

    public void KillAll()
    {
        foreach (var enemy in new List<GameObject>(alive))
        {
            if (enemy == null) continue;

            var health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
                health.TakeDamage(99999);
        }
    }
}