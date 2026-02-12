using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Behaviours/SelfDestruct")]
public class SelfDestructTimerModule : EnemyModule
{
    public float selfDestructTimer = 1.5f;
    private readonly List<GameObject> coins = new();

    public override void OnEnter(EnemyContext ctx)
    {
        ClearCoins();
        ctx.selfDestructTimer = selfDestructTimer;
        SpawnCoins();
    }

    public override void Tick(EnemyContext ctx)
    {
        ctx.selfDestructTimer -= ctx.deltaTime;

        if (ctx.selfDestructTimer <= 0)
        {
            ctx.health.TakeDamage(100);
        }
    }

    private void SpawnCoins()
    {
        if (GameManager.Instance.State != GameState.Playing) return;

        coins.Clear();

        int lives =
            GameManager.Instance.GetPlayerHealth().GetCurrentHealthValue();

        List<Vector3> spots = LevelManager.Instance.GetFreeSpots();

        if (spots == null || spots.Count == 0)
        {
            Debug.LogError("Coin: No free spots available");
            return;
        }

        for (int i = 0; i < spots.Count; i++)
        {
            int r = Random.Range(0, 100);
            if(r < 31) continue;

            Vector3 pos = spots[i];

            GameObject obj =
                ObjectPoolManager.Instance
                    .SpawnPooledObject("Coin", pos, Quaternion.identity);

            coins.Add(obj);

            obj.SetActive(true);
        }
    }

    public override void OnExit(EnemyContext ctx)
    {
        ClearCoins();
        base.OnExit(ctx);
    }

    private void ClearCoins()
    {
        if(coins.Count <= 0) return;

        foreach (GameObject coin in coins)
        {
            coin.SetActive(false);
            coins.Remove(coin);
        }
    }
}
