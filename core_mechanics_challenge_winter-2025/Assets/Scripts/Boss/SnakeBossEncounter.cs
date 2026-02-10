using UnityEngine;

public class SnakeBossEncounter : BossEncounter
{
    [SerializeField] private SnakeController snake;

    public override void StartEncounter()
    {
        started = true;
        LevelManager.Instance.LoadBossMap("Snake");
        snake.Initialize();
    }

    public override bool IsCompleted => snake.IsDead;

    public override void EndEncounter()
    {
        snake.Explode();
    }
}