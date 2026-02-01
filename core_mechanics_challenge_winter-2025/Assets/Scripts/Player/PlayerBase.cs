using System.Collections;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private HealthComponent m_health;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private Color m_hitColor = Color.red;
    [SerializeField] private float m_hitFlashTime = 0.1f;

    private Color m_defaultColor;

    private void Awake()
    {
        if (m_health == null)
            m_health = GetComponent<HealthComponent>();

        m_defaultColor = m_renderer.color;

        m_health.Health.OnDeath += OnBaseDestroyed;
        m_health.Health.OnHealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        if (m_health != null)
        {
            m_health.Health.OnDeath -= OnBaseDestroyed;
            m_health.Health.OnHealthChanged -= OnHealthChanged;
        }
    }

    private void OnHealthChanged(float current, float max)
    {
        Flash();
    }

    private void OnBaseDestroyed()
    {
        Debug.Log("PLAYER BASE DESTROYED");
        GameOver();
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
    }

    private void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(COHitFlashRoutine());
    }

    private IEnumerator COHitFlashRoutine()
    {
        m_renderer.color = m_hitColor;
        yield return new WaitForSeconds(m_hitFlashTime);
        m_renderer.color = m_defaultColor;
    }
}