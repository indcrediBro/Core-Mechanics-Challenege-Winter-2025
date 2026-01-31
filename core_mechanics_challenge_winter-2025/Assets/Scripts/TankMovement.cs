using UnityEngine;

[System.Serializable]
public class TankMovement
{
    private readonly Rigidbody2D m_rb;
    private readonly float m_speed;
    private readonly Transform m_tankBase;
    private readonly PlayerInputHandler m_input;

    public TankMovement(PlayerInputHandler _input, Rigidbody2D _rb, Transform _base, float _speed)
    {
        m_input  = _input;
        m_rb = _rb;
        m_tankBase = _base;
        m_speed = _speed;
    }

    public void Move()
    {
        m_rb.linearVelocity = m_input.m_MoveInput.normalized * m_speed;
        Rotate(m_input.m_MoveInput);
    }

    private void Rotate(Vector2 _input)
    {
        if (_input.sqrMagnitude < 0.01f)
            return;

        float angle = Mathf.Atan2(_input.y, _input.x) * Mathf.Rad2Deg + 90;
        m_tankBase.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}