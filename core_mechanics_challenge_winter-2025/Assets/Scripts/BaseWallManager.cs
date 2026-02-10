using System;
using UnityEngine;

public class BaseWallManager : MonoBehaviour
{
    [SerializeField] private Transform[] m_wallSpawnPoints;

    private void Start()
    {
        SpawnWalls();
    }

    private void SpawnWalls()
    {
        foreach (Transform wall in m_wallSpawnPoints)
        {
            GameObject wallGO = ObjectPoolManager.Instance.SpawnPooledObject("Wall", wall.position, Quaternion.Euler(0f,0f,0f));
            wallGO.transform.SetParent(transform);
            wallGO.SetActive(true);
        }
    }
}