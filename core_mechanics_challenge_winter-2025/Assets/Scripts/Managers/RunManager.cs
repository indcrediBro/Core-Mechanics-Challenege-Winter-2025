using System;

public class RunManager : Singleton<RunManager>
{
    public int CurrentWave { get; private set; }
    public int DifficultyLevel { get; private set; }

    public WaveRuntime CurrentWaveRuntime { get; private set; }

    public bool IsBossWave => (CurrentWave + 1) % 7 == 0;
    private BossEncounter activeBoss;

    public event Action<WaveRuntime> OnWaveStarted;
    public event Action OnWaveCleared;

    private void Update()
    {
        if (activeBoss != null && activeBoss.IsCompleted)
        {
            activeBoss.EndEncounter();
            activeBoss = null;
            BossDefeated();
        }
    }

    void StartBossEncounter()
    {
        activeBoss = BossFactory.Instance.SpawnBoss();
        activeBoss.StartEncounter();
    }

    public void StartRun()
    {
        CurrentWave = 0;
        DifficultyLevel = 0;
        StartWave();
    }

    public void StartWave()
    {
        if (IsBossWave)
        {
            StartBossEncounter();
            return;
        }

        int baseEnemies = 2;
        int difficultyBonus = DifficultyLevel + CurrentWave;

        CurrentWaveRuntime = new WaveRuntime
        {
            TotalEnemies = baseEnemies + difficultyBonus
        };

        OnWaveStarted?.Invoke(CurrentWaveRuntime);
    }


    public void EnemySpawned()
    {
        CurrentWaveRuntime.Spawned++;
    }

    public void EnemyKilled()
    {
        CurrentWaveRuntime.Killed++;

        if (CurrentWaveRuntime.Cleared)
            CompleteWave();
    }

    private void CompleteWave()
    {
        OnWaveCleared?.Invoke();
    }

    public void AdvanceWave()
    {
        CurrentWave++;

        if (IsBossWave)
            return;

        StartWave();
    }

    public void BossDefeated()
    {
        DifficultyLevel++;
        LevelManager.Instance.LoadRandomMap();
        AdvanceWave();
    }
}

public class WaveRuntime
{
    public int TotalEnemies;
    public int Spawned;
    public int Killed;

    public bool AllSpawned => Spawned >= TotalEnemies;
    public bool Cleared => AllSpawned && Killed >= TotalEnemies;

    public float Progress01 =>
        TotalEnemies == 0 ? 1f : (float)Killed / TotalEnemies;
}
