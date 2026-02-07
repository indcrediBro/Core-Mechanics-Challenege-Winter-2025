using System;
using UnityEngine;

public class ExplosionAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_sr;
    [SerializeField] private Sprite[] m_frames;
    [SerializeField] private float m_frameTime = 0.1f;
    [SerializeField] private bool m_destroyOnComplete = false;

    private float m_timer;
    private int m_index;

    private void OnEnable()
    {
        m_index = 0;
        m_timer = m_frameTime;
        AudioManager.Instance.PlaySound("SFX_Explode");
    }

    private void Update()
    {
        Animate();
    }

    private void DestroyOnComplete()
    {
        if (m_destroyOnComplete)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Animate()
    {
        m_timer -= Time.deltaTime;
        if (m_timer > 0f)
            return;

        m_index++;
        m_sr.sprite = m_frames[m_index];
        m_timer = m_frameTime;

        if (m_index == m_frames.Length - 1)
        {
            DestroyOnComplete();
        }
    }

    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {
        throw new NotImplementedException();
    }
}
