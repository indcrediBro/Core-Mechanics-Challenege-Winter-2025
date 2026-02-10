using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Modules")]
    [SerializeField] private EnemyModule[] m_modules;

    [Header("References")]
    [SerializeField] private Transform m_cannon;
    [SerializeField] private Transform m_firePoint;
    [SerializeField] private NavMeshAgent m_agent;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private EnemyHealth m_health;
    [SerializeField] private bool m_freezeRotation;
    private EnemyContext m_ctx;

    private void Awake()
    {
        m_ctx = new EnemyContext
        {
            self = transform,
            cannon = m_cannon,
            firePoint = m_firePoint,
            agent = m_agent,
            spriteRenderer = m_spriteRenderer,
            controller = this
        };
    }

    private void OnEnable()
    {
        m_ctx.player = GameManager.Instance.GetPlayer().transform;
        m_ctx.playerBase = GameManager.Instance.GetPlayerBase().transform;
        m_ctx.health = m_health;
        if (m_ctx.health != null)
        {
            m_ctx.health.OnDeath += HandleDeath;
            m_ctx.health.OnDamaged += HandleDamage;
        }

        foreach (var m in m_modules)
            m.OnEnter(m_ctx);
    }

    private void Update()
    {
        m_ctx.deltaTime = Time.deltaTime;

        foreach (var m in m_modules)
            m.Tick(m_ctx);
    }
    private void LateUpdate()
    {
        if(m_freezeRotation)transform.rotation = Quaternion.identity;
    }

    private void OnDisable()
    {
        if (m_ctx.health != null)
        {
            m_ctx.health.OnDeath -= HandleDeath;
            m_ctx.health.OnDamaged -= HandleDamage;
        }
        foreach (var m in m_modules)
            m.OnExit(m_ctx);
    }

    private void HandleDeath()
    {

    }

    private void HandleDamage()
    {
        foreach (var module in m_modules)
            module.OnDamage(m_ctx);
    }

    private Coroutine flashRoutine;
    private bool isFlashing;
    public bool IsFlashing => isFlashing;

    public void Flash(Color color, float duration)
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine(color, duration));
    }

    private IEnumerator FlashRoutine(Color color, float duration)
    {
        isFlashing = true;

        var sr = m_ctx.spriteRenderer;
        Color original = sr.color;

        sr.color = color;
        yield return new WaitForSeconds(duration);

        sr.color = original;
        isFlashing = false;
        flashRoutine = null;
    }
}