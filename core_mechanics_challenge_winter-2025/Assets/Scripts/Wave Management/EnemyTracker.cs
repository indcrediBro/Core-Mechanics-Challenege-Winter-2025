using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTracker
{
    private HashSet<GameObject> alive = new();

    public int ActiveCount => alive.Count;
    public bool HasBoss => alive.Any(e => e.CompareTag("Boss"));

    public void RegisterEnemy(GameObject enemy)
    {
        alive.Add(enemy);
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        alive.Remove(enemy);
    }
}