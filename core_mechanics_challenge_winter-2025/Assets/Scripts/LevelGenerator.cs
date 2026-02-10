using System.Collections.Generic;
using UnityEngine;

public enum ColorToPrefabType
{
    Player,
    PlayerBase,
    EnemySpawnPoint,
    Wall
}

[System.Serializable]
public class ColorToPrefab
{
    public string key;
    public Color color;
    public Vector3 offset;
    public ColorToPrefabType type;
}

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelManager m_levelManager;

    [Header("Level Definition")]
    [SerializeField] private Texture2D[] m_levelImages;
    [SerializeField] private ColorToPrefab[] m_colorMappings;

    private readonly List<GameObject> m_spawnedObjects = new();
    private readonly List<Vector3> m_enemySpawnPoints = new();

    private Texture2D m_levelImage;
    private Vector2 m_levelCenterOffset;

    public void GenerateBossLevel(string id)
    {
        ClearLevel();
        m_enemySpawnPoints.Clear();

        // For now: map boss ID to texture name
        foreach (var tex in m_levelImages)
        {
            if (tex.name == id)
            {
                m_levelImage = tex;
                break;
            }
        }

        if (m_levelImage == null)
        {
            Debug.LogError($"Boss map not found: {id}");
            return;
        }

        m_levelCenterOffset = new Vector2(
            -m_levelImage.width / 2f,
            -m_levelImage.height / 2f
        );

        for (int x = 0; x < m_levelImage.width; x++)
        {
            for (int y = 0; y < m_levelImage.height; y++)
            {
                GenerateTile(x, y);
            }
        }

        m_levelManager.SetSpawnPoints(m_enemySpawnPoints);
    }

    public void GenerateLevel()
    {
        ClearLevel();
        m_enemySpawnPoints.Clear();

        m_levelImage = m_levelImages[Random.Range(0, m_levelImages.Length)];

        m_levelCenterOffset = new Vector2(
            -m_levelImage.width / 2f,
            -m_levelImage.height / 2f
        );

        for (int x = 0; x < m_levelImage.width; x++)
        {
            for (int y = 0; y < m_levelImage.height; y++)
            {
                GenerateTile(x, y);
            }
        }

        m_levelManager.SetSpawnPoints(m_enemySpawnPoints);
    }

    private void GenerateTile(int x, int y)
    {
        Color pixelColor = m_levelImage.GetPixel(x, y);
        if (pixelColor.a == 0)
            return;

        foreach (var map in m_colorMappings)
        {
            if (!ColorsMatch(pixelColor, map.color))
                continue;

            Vector3 worldPos = new Vector3(
                x + m_levelCenterOffset.x,
                0f,
                y + m_levelCenterOffset.y
            ) + map.offset;

            switch (map.type)
            {
                case ColorToPrefabType.Wall:
                    GameObject wall = ObjectPoolManager.Instance
                        .SpawnPooledObject(map.key, worldPos, Quaternion.Euler(0f,0f,0f));
                    wall.SetActive(true);
                    m_spawnedObjects.Add(wall);
                    break;

                case ColorToPrefabType.EnemySpawnPoint:
                    m_enemySpawnPoints.Add(worldPos);
                    break;

                case ColorToPrefabType.Player:
                    GameManager.Instance.GetPlayer().transform.position = worldPos;
                    break;

                case ColorToPrefabType.PlayerBase:
                    GameManager.Instance.GetPlayerBase().transform.position = worldPos;
                    break;
            }

            return;
        }
    }

    private void ClearLevel()
    {
        foreach (var obj in m_spawnedObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        m_spawnedObjects.Clear();
    }

    private bool ColorsMatch(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r)
            && Mathf.Approximately(a.g, b.g)
            && Mathf.Approximately(a.b, b.b);
    }
}
