using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] private WaveDefinition[] m_waves;

    [Header("Spawning")]
    [SerializeField] private Transform[] m_spawnPoints;

    [Header("Timing")]
    [SerializeField] private float m_waveStartDelay = 1f;
    [SerializeField] private float m_waveEndDelay = 1f;

    private EnemyTracker m_tracker;
    private int m_currentWave;
    private int m_spawnedThisWave;
    private float m_spawnTimer;
    private bool m_waveActive;

    private void Awake()
    {
        m_tracker = new EnemyTracker();
    }

    private void Start()
    {
        StartWave(0);
    }

    private void Update()
    {
        if (!m_waveActive)
            return;

        HandleSpawning();
        CheckWaveEnd();
    }

    private void StartWave(int waveIndex)
    {
        if (waveIndex >= m_waves.Length)
        {
            Debug.Log("ALL WAVES COMPLETE");
            return;
        }

        m_currentWave = waveIndex;
        m_spawnedThisWave = 0;
        m_spawnTimer = 0f;
        m_waveActive = true;

        Debug.Log($"Wave {m_currentWave + 1} started");
    }

    private void HandleSpawning()
    {
        WaveDefinition wave = m_waves[m_currentWave];

        if (m_spawnedThisWave >= wave.enemyCount)
            return;

        m_spawnTimer -= Time.deltaTime;
        if (m_spawnTimer > 0f)
            return;

        SpawnEnemy(wave.enemyKey);
        m_spawnedThisWave++;
        m_spawnTimer = wave.spawnInterval;
    }

    private void SpawnEnemy(string enemyKey)
    {
        Transform spawn = m_spawnPoints[Random.Range(0, m_spawnPoints.Length)];

        GameObject enemy = ObjectPoolManager.Instance.Spawn(
            enemyKey,
            spawn.position,
            Quaternion.identity
        );

        m_tracker.RegisterEnemy();

        // Hook death callback
        HealthComponent health = enemy.GetComponent<HealthComponent>();
        health.Health.OnDeath += () =>
        {
            m_tracker.UnregisterEnemy();
        };

        Debug.Log("Spawned: " + enemy);
    }

    private void CheckWaveEnd()
    {
        WaveDefinition wave = m_waves[m_currentWave];

        if (m_spawnedThisWave < wave.enemyCount)
            return;

        if (!m_tracker.IsWaveCleared())
            return;

        m_waveActive = false;
        Invoke(nameof(EndWave), m_waveEndDelay);
    }

    private void EndWave()
    {
        Debug.Log($"Wave {m_currentWave + 1} cleared");

        // TODO: Show upgrade screen
        StartNextWave();
    }

    private void StartNextWave()
    {
        StartWave(m_currentWave + 1);
    }
}
