using UnityEngine;

public enum GameState
{
    Boot,
    Playing,
    WaveCleared,
    Upgrading,
    GameOver,
    Paused
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Core")]
    [SerializeField] private WaveManager m_waveManager;
    [SerializeField] private PlayerBase m_playerBase;
    [SerializeField] private HealthComponent m_playerHealth;

    [Header("Timing")]
    [SerializeField] private float m_upgradeDelay = 0.5f;

    public GameState State { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        State = GameState.Boot;
    }

    private void Start()
    {
        HookEvents();
        // StartGame();
        AudioManager.Instance.PlaySound("Music_MainMenu");
    }

    private void HookEvents()
    {
        if (m_playerBase != null)
            m_playerBase.GetComponent<HealthComponent>().Health.OnDeath += OnBaseDestroyed;

        if (m_playerHealth != null)
            m_playerHealth.Health.OnDeath += OnPlayerDestroyed;

        m_waveManager.OnWaveCleared += OnWaveCleared;
    }

    public void StartGame()
    {
        Debug.Log("Game Start");
        SetState(GameState.Playing);
        m_waveManager.Begin();
        AudioManager.Instance.StopSound("Music_MainMenu");
        AudioManager.Instance.PlaySound("Music_Game" + Random.Range(0, 3));
    }

    private void OnWaveCleared(int waveIndex)
    {
        Debug.Log($"Wave {waveIndex + 1} cleared");
        SetState(GameState.WaveCleared);

        Invoke(nameof(OpenUpgrades), m_upgradeDelay);
    }

    private void OpenUpgrades()
    {
        SetState(GameState.Upgrading);
        Debug.Log("Upgrade Phase");

        // TODO: show upgrade UI
        // For now, auto-continue
        CloseUpgrades();
    }

    public void CloseUpgrades()
    {
        Debug.Log("Upgrades complete");
        SetState(GameState.Playing);
        m_waveManager.StartNextWave();
    }

    private void OnPlayerDestroyed()
    {
        GameOver("Player destroyed");
    }

    private void OnBaseDestroyed()
    {
        GameOver("Base destroyed");
    }

    private void GameOver(string reason)
    {
        if (State == GameState.GameOver)
            return;

        Debug.Log($"GAME OVER: {reason}");
        SetState(GameState.GameOver);
        Time.timeScale = 0f;
        for (int i = 0; i < 3; i++)
        {
            AudioManager.Instance.StopSound("Music_Game"+i);
        }
        AudioManager.Instance.PlaySound("SFX_GameOver");
    }

    private void SetState(GameState newState)
    {
        State = newState;
        Debug.Log($"Game State → {State}");
    }
}
