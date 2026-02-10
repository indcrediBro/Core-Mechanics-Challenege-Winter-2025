using System.Collections.Generic;
using UnityEngine;

public class TurretBossEncounter : BossEncounter
{
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private Transform[] turretSpawns;

    private readonly List<EnemyHealth> turrets = new();

    public override void StartEncounter()
    {
        started = true;
        LevelManager.Instance.LoadBossMap("TurretBoss");

        foreach (var t in turretSpawns)
        {
            var turret = Instantiate(turretPrefab, t.position, Quaternion.identity);
            var health = turret.GetComponent<EnemyHealth>();
            health.OnDeath += OnTurretDestroyed;
            turrets.Add(health);
        }
    }

    private void OnTurretDestroyed()
    {
        turrets.RemoveAll(t => t == null || t.IsDead());
    }

    public override bool IsCompleted => turrets.Count == 0;

    public override void EndEncounter()
    {
        AudioManager.Instance.PlaySound("SFX_BossClear");
    }
}