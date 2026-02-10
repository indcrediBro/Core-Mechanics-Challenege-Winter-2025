using UnityEngine;

public class BonusBossEncounter : BossEncounter
{
    private float timer = 30f;
    private int coins;

    public override void StartEncounter()
    {
        started = true;
        LevelManager.Instance.LoadBossMap("Bonus");

        coins = FindObjectsOfType<Coin>().Length;
        Coin.OnCollected += OnCoinCollected;
    }

    private void OnCoinCollected()
    {
        coins--;
    }

    private void Update()
    {
        base.Update();
        timer -= Time.deltaTime;
    }

    public override bool IsCompleted =>
        coins <= 0 || timer <= 0f;

    public override void EndEncounter()
    {
        Coin.OnCollected -= OnCoinCollected;
    }
}