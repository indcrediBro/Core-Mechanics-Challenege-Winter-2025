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

public class GameManager : Singleton<GameManager>
{
    [Header("Core")]
    [SerializeField] private WaveManager m_waveManager;
    [SerializeField] private PlayerTank m_player;
    [SerializeField] private PlayerBase m_playerBase;
    [SerializeField] private HealthComponent m_playerHealth;

    [Header("Timing")]
    [SerializeField] private float m_upgradeDelay = 0.5f;



    public GameState State { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        State = GameState.Boot;
    }

    private void Start()
    {
        HookEvents();

        m_player.gameObject.SetActive(false);
        m_playerBase.gameObject.SetActive(false);
    }

    private void HookEvents()
    {
        if (m_playerBase != null)
            m_playerBase.GetComponent<HealthComponent>().HealthOld.OnDeath += OnBaseDestroyed;

        if (m_playerHealth != null)
            m_playerHealth.HealthOld.OnDeath += OnPlayerDestroyed;

        m_waveManager.OnWaveCleared += OnWaveCleared;
    }

    public void StartGame()
    {
        m_player.gameObject.SetActive(true);
        m_playerBase.gameObject.SetActive(true);
        Debug.Log("Game Start");
        SetState(GameState.Playing);
        m_waveManager.Begin();
        MusicManager.Instance.StopAllMusic();
        MusicManager.Instance.PlayInGameTrack();
    }

    private void OnWaveCleared(int waveIndex)
    {
        Debug.Log($"Wave {waveIndex + 1} cleared");
        SetState(GameState.WaveCleared);

        Invoke(nameof(OpenUpgrades), m_upgradeDelay);
    }

    public void OpenUpgrades()
    {
        SetState(GameState.Upgrading);
        Debug.Log("Upgrade Phase");
        Time.timeScale = 0;
        UIManager.Instance.ShowUpgrades();
    }

    public void CloseUpgrades()
    {
        Debug.Log("Upgrades complete");
        SetState(GameState.Playing);
        UIManager.Instance.Show(UIState.HUD);
        Time.timeScale = 1;
        m_waveManager.StartNextWave();
    }

    private void OnPlayerDestroyed()
    {
        GameOver("Player destroyed!");
    }

    private void OnBaseDestroyed()
    {
        GameOver("Base destroyed!");
    }

    private void GameOver(string reason)
    {
        if (State == GameState.GameOver)
            return;

        Debug.Log($"GAME OVER: {reason}");

        UIManager.Instance.GameOver(reason);
        SetState(GameState.GameOver);
        Time.timeScale = 0f;
        MusicManager.Instance.StopAllMusic();
        AudioManager.Instance.PlaySound("SFX_GameOver");
    }

    private void SetState(GameState newState)
    {
        State = newState;
    }

    public PlayerTank GetPlayer()
    {
        return m_player;
    }

    public void TogglePause()
    {
        if (State is not (GameState.Playing or GameState.Paused)) return;

        if (State == GameState.Paused)
        {
            SetState(GameState.Playing);
            UIManager.Instance.Show(UIState.HUD);
        }
        else
        {
            SetState(GameState.Paused);
        }
    }

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public PlayerBase GetPlayerBase()
    {
        return m_playerBase;
    }
}

