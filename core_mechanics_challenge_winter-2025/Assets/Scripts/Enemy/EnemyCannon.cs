using UnityEngine;

public class EnemyCannon
{
    private readonly Transform m_cannon;
    private readonly float m_rotateSpeed;

    public EnemyCannon(Transform _cannon, float _rotateSpeed)
    {
        m_cannon = _cannon;
        m_rotateSpeed = _rotateSpeed;
    }

    public void AimAt(Vector3 _target, float _deltaTime)
    {
        Vector2 dir = _target - m_cannon.position;
        if (dir.sqrMagnitude < 0.001f)
            return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);

        m_cannon.rotation = Quaternion.RotateTowards(
            m_cannon.rotation,
            targetRot,
            m_rotateSpeed * _deltaTime
        );
    }
}