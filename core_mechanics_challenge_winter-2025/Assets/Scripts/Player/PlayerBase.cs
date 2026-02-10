using System.Collections;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private BaseHealth m_health;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private Color m_hitColor = Color.red;
    [SerializeField] private float m_hitFlashTime = 0.1f;

    private Color m_defaultColor;

    private void OnEnable()
    {
        m_defaultColor = m_renderer.color;

        m_health.OnDeath += OnBaseDestroyed;
        m_health.OnHealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        if (m_health != null)
        {
            m_health.OnDeath -= OnBaseDestroyed;
            m_health.OnHealthChanged -= OnHealthChanged;
        }
    }

    private void OnHealthChanged(int current, int max)
    {
        Flash();
    }

    private void OnBaseDestroyed()
    {
        Debug.Log("PLAYER BASE DESTROYED");
        // GameOver();
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