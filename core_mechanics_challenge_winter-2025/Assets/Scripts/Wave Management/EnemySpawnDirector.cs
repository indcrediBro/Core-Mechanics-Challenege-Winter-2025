using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnDirector : Singleton<EnemySpawnDirector>
{
    [Header("Data")]
    [SerializeField] private EnemyCatalog catalog;

    [Header("Timing")]
    [SerializeField] private Vector2 spawnIntervalRange = new(0.5f, 2f);

    private EnemyTracker tracker;
    private float spawnTimer;
    private bool spawningEnabled;

    protected override void Awake()
    {
        base.Awake();
        tracker = new EnemyTracker();
    }

    private void OnEnable()
    {
        RunManager.Instance.OnWaveStarted += OnWaveStarted;
        RunManager.Instance.OnWaveCleared += OnWaveCleared;
    }

    private void OnDisable()
    {
        RunManager.Instance.OnWaveStarted -= OnWaveStarted;
        RunManager.Instance.OnWaveCleared -= OnWaveCleared;
    }

    private void OnWaveStarted(WaveRuntime wave)
    {
        tracker = new EnemyTracker();
        spawningEnabled = true;
        spawnTimer = 0f;
    }

    private void OnWaveCleared()
    {
        spawningEnabled = false;
    }

    private void Update()
    {
        if (!spawningEnabled)
            return;

        if (GameManager.Instance.State != GameState.Playing)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            TrySpawn();
            spawnTimer = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
        }
    }

    private void TrySpawn()
    {
        var wave = RunManager.Instance.CurrentWaveRuntime;
        if (wave == null)
            return;

        if (wave.AllSpawned)
            return;

        if (tracker.ActiveCount >= GetMaxEnemies())
            return;

        string enemyKey = PickEnemy();
        Spawn(enemyKey);

        RunManager.Instance.EnemySpawned();
    }

    private string PickEnemy()
    {
        if (RunManager.Instance.IsBossWave)
        {
            return catalog.bossEnemies[
                Random.Range(0, catalog.bossEnemies.Count)
            ];
        }

        return catalog.normalEnemies[
            Random.Range(0, catalog.normalEnemies.Count)
        ];
    }

    private int GetMaxEnemies()
    {
        if (RunManager.Instance.IsBossWave)
            return 1;

        return 5 + RunManager.Instance.DifficultyLevel * 3;
    }

    private void Spawn(string enemyKey)
    {
        List<Vector3> points = LevelManager.Instance.GetSpawnPoints();
        Vector3 spawn = points[Random.Range(0, points.Count)];

        GameObject enemy = ObjectPoolManager.Instance
            .SpawnPooledObject(enemyKey, spawn, Quaternion.identity);

        tracker.RegisterEnemy(enemy);

        var health = enemy.GetComponent<EnemyHealth>();

        void OnEnemyDeath()
        {
            health.OnDeath -= OnEnemyDeath;
            tracker.UnregisterEnemy(enemy);
            RunManager.Instance.EnemyKilled();

            if (enemy.CompareTag("Boss"))
                RunManager.Instance.BossDefeated();
        }

        health.OnDeath += OnEnemyDeath;
        enemy.SetActive(true);
    }

    public void KillAllActiveEnemies()
    {
        tracker.KillAll();
    }
}