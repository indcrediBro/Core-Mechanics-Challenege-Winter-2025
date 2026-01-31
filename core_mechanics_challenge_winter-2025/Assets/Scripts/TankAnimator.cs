using System;
using UnityEngine;

[Serializable]
public class TankAnimator
{
    [SerializeField] private SpriteRenderer m_sr;
    [SerializeField] private Sprite[] m_frames;
    [SerializeField] private float m_frameTime = 0.1f;
    private PlayerInputHandler m_input;

    private float m_timer;
    private int m_index;
    private bool m_moving;

    public void Initialize(PlayerInputHandler _input)
    {
        m_input = _input;
    }

    public void SetMoveAmount()
    {
        m_moving = m_input.m_MoveInput.SqrMagnitude() > 0.01f;
    }

    public void Animate()
    {
        if (!m_moving)
            return;

        m_timer -= Time.deltaTime;
        if (m_timer > 0f)
            return;

        m_index = (m_index + 1) % m_frames.Length;
        m_sr.sprite = m_frames[m_index];
        m_timer = m_frameTime;
    }
}
