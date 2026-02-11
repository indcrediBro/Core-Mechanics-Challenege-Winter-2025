using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private NavMeshSurface navMeshSurface;

    private List<Vector3> spawnPoints = new ();
    private List<Vector3> freeSpots = new ();

    public List<Vector3> GetSpawnPoints() => spawnPoints;

    public void SetSpawnPoints(List<Vector3> _spawnPoints)
    {
        spawnPoints = _spawnPoints.ToList();
    }

    public void SetFreeSpots(List<Vector3> _freeSpots)
    {
        freeSpots = _freeSpots;
    }

    public void LoadBossMap(string mapId)
    {
        levelGenerator.GenerateBossLevel(mapId);
        navMeshSurface.BuildNavMesh();
    }

    public void LoadRandomMap()
    {
        levelGenerator.GenerateLevel();
        navMeshSurface.BuildNavMesh();
    }

    public List<Vector3> GetFreeSpots()
    {
        return freeSpots;
    }
}