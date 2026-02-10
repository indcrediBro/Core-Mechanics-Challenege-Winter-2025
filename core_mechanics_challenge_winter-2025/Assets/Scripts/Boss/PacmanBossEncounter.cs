using UnityEngine;

public class PacmanBossEncounter : BossEncounter
{
    private int pelletsRemaining;

    public override void StartEncounter()
    {
        started = true;
        LevelManager.Instance.LoadBossMap("Pacman");

        pelletsRemaining = FindObjectsOfType<Pellet>().Length;
        Pellet.OnCollected += HandlePellet;
    }

    private void HandlePellet()
    {
        pelletsRemaining--;
    }

    public override bool IsCompleted => pelletsRemaining <= 0;

    public override void EndEncounter()
    {
        Pellet.OnCollected -= HandlePellet;
    }
}