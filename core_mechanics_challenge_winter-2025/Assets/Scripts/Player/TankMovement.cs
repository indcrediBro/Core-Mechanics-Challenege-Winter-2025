using UnityEngine;

[System.Serializable]
public class TankMovement
{
    private readonly Rigidbody m_rb;
    private readonly float m_speed;
    private readonly Transform m_tankBase;
    private readonly PlayerInputHandler m_input;

    public TankMovement(PlayerInputHandler _input, Rigidbody _rb, Transform _base, float _speed)
    {
        m_input = _input;
        m_rb = _rb;
        m_tankBase = _base;
        m_speed = _speed;

        m_rb.constraints =
            RigidbodyConstraints.FreezeRotation |
            RigidbodyConstraints.FreezePositionY;
    }

    public void Move()
    {
        if (GameManager.Instance.State != GameState.Playing)
        {
            m_rb.linearVelocity = Vector3.zero;
            AudioManager.Instance.StopSound("SFX_Move");
            return;
        }

        Vector2 input2D = m_input.m_MoveInput;

        if (input2D.sqrMagnitude > 0.01f)
        {
            if (!AudioManager.Instance.IsPlaying("SFX_Move"))
                AudioManager.Instance.PlaySound("SFX_Move");
        }
        else
        {
            AudioManager.Instance.StopSound("SFX_Move");
        }

        Vector3 moveDir = new Vector3(input2D.x, 0f, input2D.y).normalized;
        m_rb.linearVelocity = moveDir * m_speed;

        Rotate(input2D);
    }

    private void Rotate(Vector2 input)
    {
        if (input.sqrMagnitude < 0.01f)
            return;

        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        m_tankBase.parent.rotation = Quaternion.Euler(0f, angle,0f);
    }
}