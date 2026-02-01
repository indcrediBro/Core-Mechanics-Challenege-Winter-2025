using UnityEngine;
using Pathfinding;

public class EnemyMovement
{
    private readonly Transform m_transform;
    private readonly Seeker m_seeker;
    private readonly float m_speed;
    private readonly float m_nextWaypointDist;

    private Path m_path;
    private int m_waypointIndex;

    public EnemyMovement(
        Transform _transform,
        Seeker _seeker,
        float _speed,
        float _nextWaypointDist
    )
    {
        m_transform = _transform;
        m_seeker = _seeker;
        m_speed = _speed;
        m_nextWaypointDist = _nextWaypointDist;
    }

    public void RequestPath(Vector3 _target)
    {
        if (!m_seeker.IsDone())
            return;

        m_seeker.StartPath(m_transform.position, _target, OnPathComplete);
    }

    private void OnPathComplete(Path _p)
    {
        if (_p.error)
            return;

        m_path = _p;
        m_waypointIndex = 0;
    }

    public void Tick(float _deltaTime)
    {
        if (m_path == null || m_waypointIndex >= m_path.vectorPath.Count)
            return;

        Vector2 dir = ((Vector2)m_path.vectorPath[m_waypointIndex] -
                       (Vector2)m_transform.position);

        if (dir.sqrMagnitude < m_nextWaypointDist * m_nextWaypointDist)
        {
            m_waypointIndex++;
            return;
        }

        m_transform.Translate(dir.normalized * m_speed * _deltaTime, Space.World);
    }
}