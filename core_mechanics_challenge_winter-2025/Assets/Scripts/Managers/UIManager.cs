using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public enum UIState
{
    MainMenu,
    HUD,
    Pause,
    Upgrade,
    GameOver
}

public class UIManager : Singleton<UIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_hud;
    [SerializeField] private GameObject m_pause;
    [SerializeField] private GameObject m_upgrade;
    [SerializeField] private GameObject m_gameOver;
    [SerializeField] private GameObject m_fakeLoadingScreen;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text m_gameOverReasonText;
    [SerializeField] private Slider m_waveSlider;
    [SerializeField] private TMP_Text m_waveText;
    [SerializeField] private TMP_Text m_difficultyText;
    [SerializeField] private TMP_Text m_bossText;
    [SerializeField] private TMP_Text[] m_scoresUI;
    [SerializeField] private TMP_Text[] m_bestScoresUI;
    [SerializeField] private TMP_Text[] m_livesUI;
    [SerializeField] private TMP_Text[] m_baseLivesUI;
    [SerializeField] private TMP_Text m_powerUpText;
    [SerializeField] private CanvasGroup m_powerUpCanvas;
    [SerializeField] private float m_powerUpDuration = 2f;

    private Dictionary<UIState, GameObject> m_panels;
    private float sliderVelocity;

    protected override void Awake()
    {
        base.Awake();
        InitializeUI();
    }

    private void OnDestroy()
    {
        if (RunManager.Instance != null)
        {
            RunManager.Instance.OnWaveStarted -= OnWaveStarted;
            RunManager.Instance.OnWaveCleared -= OnWaveCleared;
            RunManager.Instance.OnBossWaveStarted -= OnBossWaveStarted;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GetPlayerHealth().OnDamaged -= UpdateLivesUI;
            GameManager.Instance.OnPowerUpPicked -= ShowPowerUp;
        }
    }

    private void InitializeUI()
    {
        m_panels = new Dictionary<UIState, GameObject>
        {
            { UIState.MainMenu, m_mainMenu },
            { UIState.HUD, m_hud },
            { UIState.Pause, m_pause },
            { UIState.Upgrade, m_upgrade },
            { UIState.GameOver, m_gameOver }
        };

        m_fakeLoadingScreen.SetActive(true);
        Show(UIState.MainMenu);
        m_gameOverReasonText.text = string.Empty;

        RunManager.Instance.OnWaveStarted += OnWaveStarted;
        RunManager.Instance.OnWaveCleared += OnWaveCleared;
        RunManager.Instance.OnBossWaveStarted += OnBossWaveStarted;
        GameManager.Instance.OnPowerUpPicked += ShowPowerUp;
        GameManager.Instance.GetPlayerHealth().OnDamaged += UpdateLivesUI;
    }

    public void Show(UIState state)
    {
        foreach (var panel in m_panels.Values)
            panel.SetActive(false);

        m_panels[state].SetActive(true);

        Time.timeScale = state == UIState.Pause ? 0f : 1f;
    }

    public void Play()
    {
        Show(UIState.HUD);
        GameManager.Instance.StartGame();
        UpdateLivesUI();
    }

    public void Resume() => Show(UIState.HUD);
    public void Quit() => Application.Quit();
    public void QuitToMenu() => Show(UIState.MainMenu);
    public void ShowUpgrades() => Show(UIState.Upgrade);

    private void OnWaveStarted(WaveRuntime wave)
    {
        m_waveSlider.value = 0f;
        m_waveText.text = $"Wave {RunManager.Instance.CurrentWave + 1}";

        string[] names = { "Easy", "Normal", "Hard", "Expert", "Master", "God" };
        int index = Mathf.Clamp(RunManager.Instance.DifficultyLevel, 0, names.Length - 1);
        m_difficultyText.text = names[index];

        if (m_bossText != null)
            m_bossText.gameObject.SetActive(false);
    }

    private void OnBossWaveStarted()
    {
        if (m_bossText == null) return;

        m_bossText.gameObject.SetActive(true);
        m_bossText.text = "BOSS WAVE";
    }

    private void OnWaveCleared()
    {
        ShowUpgrades();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Show(UIState.Pause);
            GameManager.Instance.TogglePause();
        }

        if (GameManager.Instance.State != GameState.Playing)
            return;

        var wave = RunManager.Instance.CurrentWaveRuntime;
        if (wave == null)
            return;

        float target = wave.Progress01;
        m_waveSlider.value = Mathf.SmoothDamp(
            m_waveSlider.value,
            target,
            ref sliderVelocity,
            0.2f
        );
    }

    public void GameOver(string reason)
    {
        m_gameOverReasonText.text = reason;
        Show(UIState.GameOver);
    }

    public void DisableLoadingScreen()
    {
        m_fakeLoadingScreen.SetActive(false);
        MusicManager.Instance.StopAllMusic();
        MusicManager.Instance.PlayMainMenuMusic();
    }

    public void UpdateScore()
    {
        UpdateCurrentScoresUI();
        UpdateBestScoresUI();
    }

    private void UpdateCurrentScoresUI()
    {
        foreach (var scoreText in m_scoresUI)
            scoreText.text = ScoreManager.Instance.Score.ToString();
    }

    private void UpdateBestScoresUI()
    {
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        int current = ScoreManager.Instance.Score;

        if (current > highscore)
        {
            highscore = current;
            PlayerPrefs.SetInt("HighScore", highscore);
        }

        foreach (var scoreText in m_bestScoresUI)
            scoreText.text = highscore.ToString();
    }

    public void UpdateLivesUI()
    {
        int l = GameManager.Instance.GetPlayerHealth().GetCurrentHealthValue();
        Color c = l <= 1 ? Color.red : Color.white;

        foreach (var liveText in m_livesUI)
        {
            liveText.text = l.ToString();
            liveText.color = c;
        }

        int m = GameManager.Instance.GetPlayerBase().GetComponent<Health>().GetCurrentHealthValue();
        Color d = m <= 1 ? Color.red : Color.white;

        foreach (var baseText in m_baseLivesUI)
        {
            baseText.text = m.ToString();
            baseText.color = d;
        }
    }

    private Coroutine powerRoutine;

    private void ShowPowerUp(string powerName)
    {
        if (powerRoutine != null)
            StopCoroutine(powerRoutine);

        powerRoutine = StartCoroutine(PowerUpRoutine(powerName));
    }

    private IEnumerator PowerUpRoutine(string powerName)
    {
        m_powerUpText.text = powerName;
        m_powerUpCanvas.alpha = 0f;

        float t = 0f;

        // Fade in + slide
        while (t < 1f)
        {
            t += Time.deltaTime * 4f;
            m_powerUpCanvas.alpha = t;
            yield return null;
        }

        yield return new WaitForSeconds(m_powerUpDuration);

        t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime * 2f;
            m_powerUpCanvas.alpha = t;
            yield return null;
        }

        m_powerUpCanvas.alpha = 0f;
    }
}
