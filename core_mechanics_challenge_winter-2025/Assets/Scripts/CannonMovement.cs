using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class CannonMovement
{
    private readonly Transform m_target;
    private readonly float m_speed;

    public CannonMovement(Transform _target, float _speed)
    {
        m_target = _target;
        m_speed = _speed;
    }

    public void Rotate(Vector2 _direction, float deltaTime)
    {
        if (_direction.sqrMagnitude < 0.001f)
            return;

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        m_target.rotation = Quaternion.RotateTowards(
            m_target.rotation,
            targetRotation,
            m_speed * deltaTime
        );
    }
}
