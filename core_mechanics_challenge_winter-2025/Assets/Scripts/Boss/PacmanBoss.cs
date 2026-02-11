using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PacmanBoss : BossBase
{
    private readonly List<PacmanPellet> pellets = new();
    private PacmanPellet currentTarget;

    [Header("Behavior")]
    [SerializeField] private float fearDuration = 1.2f;

    private float fearTimer;

    protected override void OnEnable()
    {
        base.OnEnable();
        SpawnPellets();
        currentTarget = null;
        fearTimer = 0f;
    }

    private void OnDisable()
    {
        foreach (var pacmanPellet in pellets)
        {
            pacmanPellet.gameObject.SetActive(false);
        }
        pellets.Clear();
    }

    private void Update()
    {
        if (health.IsDead())
            return;

        CleanupPelletList();

        // LOS triggers fear memory
        if (HasLineOfSight())
        {
            fearTimer = fearDuration;
        }

        // Fear movement
        if (fearTimer > 0f)
        {
            fearTimer -= Time.deltaTime;
            MoveAway(player.position);
            return;
        }

        // Acquire pellet target if needed
        if (currentTarget == null && pellets.Count > 0)
        {
            currentTarget = GetClosestPellet();
        }

        if (currentTarget == null)
            return;

        MoveTowards(currentTarget.transform.position);
    }

    private void CleanupPelletList()
    {
        for (int i = pellets.Count - 1; i >= 0; i--)
        {
            if (pellets[i] == null)
                pellets.RemoveAt(i);
        }

        if (currentTarget != null && !pellets.Contains(currentTarget))
            currentTarget = null;
    }

    private PacmanPellet GetClosestPellet()
    {
        float minDist = float.MaxValue;
        PacmanPellet closest = null;

        foreach (var p in pellets)
        {
            float d = Vector3.SqrMagnitude(p.transform.position - transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = p;
            }
        }

        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PacmanPellet pellet))
            return;

        if (pellets.Contains(pellet))
            pellets.Remove(pellet);

        if (pellet == currentTarget)
            currentTarget = null;

        GameManager.Instance
            .GetPlayerHealth()
            .TakeDamage(pellet.damage);

        pellet.gameObject.SetActive(false);
    }

    private void SpawnPellets()
    {
        if(GameManager.Instance.State != GameState.Playing) return;

        pellets.Clear();

        int lives =
            GameManager.Instance.GetPlayerHealth().GetCurrentHealthValue();

        List<Vector3> spots = LevelManager.Instance.GetFreeSpots();

        if (spots == null || spots.Count == 0)
        {
            Debug.LogError("PacmanBoss: No free spots available");
            return;
        }

        for (int i = 0; i < lives; i++)
        {
            Vector3 pos = spots[Random.Range(0, spots.Count)];

            GameObject obj =
                ObjectPoolManager.Instance
                .SpawnPooledObject("Pellet", pos, Quaternion.identity);

            if (obj.TryGetComponent(out PacmanPellet pellet))
                pellets.Add(pellet);

            obj.SetActive(true);
        }
    }
}
