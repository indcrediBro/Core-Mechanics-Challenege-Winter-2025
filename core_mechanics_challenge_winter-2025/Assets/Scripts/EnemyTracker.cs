public class EnemyTracker
{
    public int AliveCount { get; private set; }

    public void RegisterEnemy()
    {
        AliveCount++;
    }

    public void UnregisterEnemy()
    {
        AliveCount--;
        if (AliveCount < 0)
            AliveCount = 0;
    }

    public bool IsWaveCleared()
    {
        return AliveCount == 0;
    }
}