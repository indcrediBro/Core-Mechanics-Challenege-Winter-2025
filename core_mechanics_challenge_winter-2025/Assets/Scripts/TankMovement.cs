using UnityEngine;

[System.Serializable]
public class TankMovement
{
    private readonly Rigidbody2D m_rb;
    private readonly float m_speed;

    public TankMovement(Rigidbody2D _rb, float _speed)
    {
        m_rb = _rb;
        m_speed = _speed;
    }

    public void Move(Vector2 _input)
    {
        m_rb.linearVelocity = _input.normalized * m_speed;
        Rotate(_input);
    }

    private void Rotate(Vector2 _input)
    {
        if (_input.sqrMagnitude < 0.01f)
            return;

        float angle = Mathf.Atan2(_input.y, _input.x) * Mathf.Rad2Deg;
        m_rb.rotation = angle + 90;
    }
}