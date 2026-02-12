using System;

public class RunManager : Singleton<RunManager>
{
    public int CurrentWave { get; private set; }
    public int DifficultyLevel { get; private set; }

    public WaveRuntime CurrentWaveRuntime { get; private set; }

    public bool IsBossWave => (CurrentWave + 1) % 7 == 0;

    public event Action<WaveRuntime> OnWaveStarted;
    public event Action OnWaveCleared;
    public event Action OnBossWaveStarted;

    public bool freezeEnemies = false;

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
            CurrentWaveRuntime = new WaveRuntime
            {
                TotalEnemies = 1
            };

            OnBossWaveStarted?.Invoke();
        }
        else
        {
            int baseEnemies = 6;
            int difficultyBonus = DifficultyLevel + CurrentWave;

            CurrentWaveRuntime = new WaveRuntime
            {
                TotalEnemies = baseEnemies + difficultyBonus
            };
        }

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
        OnUpgradePhaseStarted?.Invoke();
    }
    public event Action OnUpgradePhaseStarted;

    public void AdvanceWave()
    {
        bool wasBossWave = IsBossWave;

        CurrentWave++;

        if (wasBossWave)
        {
            LevelManager.Instance.LoadRandomMap();
            DifficultyLevel++;
        }

        StartWave();
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
