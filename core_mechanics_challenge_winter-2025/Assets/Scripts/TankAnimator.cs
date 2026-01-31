using System;
using UnityEngine;

[Serializable]
public class TankAnimator
{
    [SerializeField] private SpriteRenderer m_sr;
    [SerializeField] private Sprite[] m_frames;
    [SerializeField] private float m_frameTime = 0.1f;

    private float m_timer;
    private int m_index;
    private bool m_moving;

    public void SetMoveAmount(float _sqr)
    {
        m_moving = _sqr > 0.01f;
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
