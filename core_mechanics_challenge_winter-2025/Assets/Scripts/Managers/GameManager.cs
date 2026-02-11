using System;
using Unity.VisualScripting;
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
    [SerializeField] private PlayerController m_player;
    [SerializeField] private PlayerBase m_playerBase;
    [SerializeField] private PlayerHealth m_playerHealth;

    [Header("Timing")]
    [SerializeField] private float m_upgradeDelay = 0.5f;

    public GameState State { get; private set; }
    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnBombExplode;
    public event Action OnShotFired;
    public event Action<string> OnPowerUpPicked;

    public void RaisePowerUp(string powerName)
    {
        OnPowerUpPicked?.Invoke(powerName);
    }
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
            m_playerBase.GetComponent<BaseHealth>().OnDeath += OnBaseDestroyed;

        if (m_playerHealth != null)
            m_playerHealth.OnDeath += OnPlayerDestroyed;

        RunManager.Instance.OnWaveCleared += OnWaveCleared;
    }

    public void StartGame()
    {
        m_player.gameObject.SetActive(true);
        m_playerBase.gameObject.SetActive(true);
        Debug.Log("Game Start");
        OnGameStart?.Invoke();
        SetState(GameState.Playing);
        LevelManager.Instance.LoadRandomMap();
        RunManager.Instance.StartRun();
        MusicManager.Instance.StopAllMusic();
        MusicManager.Instance.PlayInGameTrack();
    }

    private void OnWaveCleared()
    {
        SetState(GameState.WaveCleared);
    }

    public void OpenUpgrades()
    {
        SetState(GameState.Upgrading);
        Debug.Log("Upgrade Phase");
        Time.timeScale = 0;
        // UIManager.Instance.ShowUpgrades();
    }

    public void CloseUpgrades()
    {
        Debug.Log("Upgrades complete");

        SetState(GameState.Playing);
        UIManager.Instance.Show(UIState.HUD);
        Time.timeScale = 1;
        AudioManager.Instance.PlaySound("SFX_Powerup");
        RunManager.Instance.AdvanceWave();
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
        OnGameOver?.Invoke();
    }

    private void SetState(GameState newState)
    {
        State = newState;
    }

    public PlayerController GetPlayer()
    {
        return m_player;
    }

    public PlayerHealth GetPlayerHealth()
    {
        return m_playerHealth;
    }

    public void TogglePause()
    {
        switch (State)
        {
            case GameState.Paused:
                SetState(GameState.Playing);
                UIManager.Instance.Show(UIState.HUD);
                break;
            case GameState.Playing:
                SetState(GameState.Paused);
                break;
            default:
                break;
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

    public void Flash()
    {
        OnBombExplode?.Invoke();
    }

    public void ShotFired()
    {
        OnShotFired?.Invoke();
    }
}

