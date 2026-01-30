using System;
using UnityEngine;

public class TankAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_tankSR;
    [SerializeField] private Sprite[] m_allSprite;
    [SerializeField] private float m_speed;

    private int m_index;
    private float m_timer;
    
    private void OnEnable()
    {
        //Listen to Player movement input
        m_timer =  m_speed;
    }

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        if (m_timer > 0)
        {
            m_timer -= Time.deltaTime;

        }
        else if (m_timer <= 0)
        {
            m_index = (m_index + 1) % m_allSprite.Length;
            m_tankSR.sprite = m_allSprite[m_index];
            m_timer = m_speed;
        }
    }
}
