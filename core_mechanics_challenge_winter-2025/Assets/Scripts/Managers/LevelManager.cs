using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private NavMeshSurface navMeshSurface;

    private List<Vector3> spawnPoints = new ();

    public List<Vector3> GetSpawnPoints() => spawnPoints;

    public void SetSpawnPoints(List<Vector3> _spawnPoints)
    {
        spawnPoints = _spawnPoints.ToList();
    }

    public void AddSpawnPoint(Vector2 _spawnPoint)
    {
        spawnPoints.Add(_spawnPoint);
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
}