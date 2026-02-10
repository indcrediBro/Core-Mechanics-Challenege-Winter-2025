using UnityEngine;

public class SimpleAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_sr;
    [SerializeField] private Sprite[] m_frames;
    [SerializeField] private float m_frameTime = 0.1f;
    [SerializeField] private bool m_destroyOnComplete = false;
    [SerializeField] private bool m_loop = false;

    private float m_timer;
    private int m_index;

    private void OnEnable()
    {
        ResetAnimation();
    }

    private void Update()
    {
        Animate();
    }

    private void ResetAnimation()
    {
        m_index = 0;
        m_timer = m_frameTime;

        if (m_frames.Length > 0)
            m_sr.sprite = m_frames[0];
    }

    private void Animate()
    {
        if (m_frames.Length == 0)
            return;

        m_timer -= Time.deltaTime;
        if (m_timer > 0f)
            return;

        m_timer = m_frameTime;
        m_index++;

        // Finished animation
        if (m_index >= m_frames.Length)
        {
            if (m_loop)
            {
                m_index = 0;
                m_sr.sprite = m_frames[m_index];
            }
            else
            {
                Complete();
            }
            return;
        }

        m_sr.sprite = m_frames[m_index];
    }

    private void Complete()
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
}