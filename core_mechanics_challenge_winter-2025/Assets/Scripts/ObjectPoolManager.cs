using UnityEngine;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance { get; private set; }

        [System.Serializable]
        private struct PoolConfig
        {
            public string key;
            public GameObject prefab;
            public int initialSize;
        }

        [SerializeField] private PoolConfig[] m_pools;

        private readonly Dictionary<string, ObjectPool> m_poolMap = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            foreach (var config in m_pools)
            {
                Transform parent = new GameObject($"Pool_{config.key}").transform;
                parent.SetParent(transform);

                ObjectPool pool = new ObjectPool(
                    config.prefab,
                    config.initialSize,
                    parent
                );

                m_poolMap.Add(config.key, pool);
            }
        }

        public GameObject Spawn(string _key, Vector3 _pos, Quaternion _rot)
        {
            return m_poolMap[_key].Spawn(_pos, _rot);
        }

        public void Despawn(string key, GameObject go)
        {
            m_poolMap[key].Despawn(go);
        }
    }
}