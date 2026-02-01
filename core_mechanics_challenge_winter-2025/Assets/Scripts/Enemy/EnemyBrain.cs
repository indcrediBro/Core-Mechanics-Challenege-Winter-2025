using UnityEngine;

[System.Serializable]
public class EnemyBrain
{
    private readonly Transform m_self;
    private readonly Transform m_player;
    private readonly Transform m_base;
    private readonly float m_playerAggroRange;

    public EnemyBrain(
        Transform _self,
        Transform _player,
        Transform _playerBase,
        float _playerAggroRange
    )
    {
        m_self = _self;
        m_player = _player;
        m_base = _playerBase;
        m_playerAggroRange = _playerAggroRange;
    }

    public Transform GetTarget()
    {
        if (m_player == null)
            return m_base;

        float dist = Vector2.Distance(m_self.position, m_player.position);
        return dist <= m_playerAggroRange ? m_player : m_base;
    }
}