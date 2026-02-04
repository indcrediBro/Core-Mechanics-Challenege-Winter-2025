using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly GameObject m_prefab;
    private readonly Transform m_parent;

    private readonly Queue<GameObject> m_available = new();
    private readonly List<GameObject>  m_all = new();

    public ObjectPool(GameObject _prefab, int _initialSize, Transform _parent)
    {
        m_prefab = _prefab;
        m_parent = _parent;

        Warm(_initialSize);
    }

    private void Warm(int _count)
    {
        for (int i = 0; i < _count; i++)
            CreateNew();
    }

    private GameObject CreateNew()
    {
        GameObject go = Object.Instantiate(m_prefab, m_parent);
        go.SetActive(false);

        m_all.Add(go);
        m_available.Enqueue(go);

        return go;
    }

    public GameObject Spawn(Vector3 _position, Quaternion _rotation)
    {
        GameObject go = m_available.Count > 0
            ? m_available.Dequeue()
            : CreateNew();

        go.transform.SetPositionAndRotation(_position, _rotation);
        go.SetActive(true);

        if (go.TryGetComponent(out IPoolable poolable))
            poolable.OnSpawn();

        return go;
    }

    public void Despawn(GameObject _go)
    {
        if (_go.TryGetComponent(out IPoolable poolable))
            poolable.OnDespawn();

        _go.SetActive(false);
        m_available.Enqueue(_go);
    }
}