using System;
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
    [SerializeField] private TMP_Text[] m_scoresUI, m_bestScoresUI, m_livesUI;

    private Dictionary<UIState, GameObject> m_panels;
    protected override void Awake()
    {
        base.Awake();
        InitializeUI();
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
        m_gameOverReasonText.text = String.Empty;
        RunManager.Instance.OnWaveStarted += OnWaveStarted;
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
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        Show(UIState.HUD);
    }

    public void QuitToMenu()
    {
        Show(UIState.MainMenu);
    }

    public void ShowUpgrades()
    {
        Show(UIState.Upgrade);
    }

    private void OnWaveStarted(WaveRuntime wave)
    {
        m_waveSlider.value = 0f;
        m_waveText.text = $"{RunManager.Instance.CurrentWave + 1}";
        string difficultyName = String.Empty;
        switch (RunManager.Instance.DifficultyLevel)
        {
            case 0:
                difficultyName = "Easy";
                break;
            case 1:
                difficultyName = "Normal";
                break;
            case 2:
                difficultyName = "Hard";
                break;
            case 3:
                difficultyName = "Expert";
                break;
            case 4:
                difficultyName = "Master";
                break;
            default:
                difficultyName = "God";
                break;
        }
        m_difficultyText.text = difficultyName;
    }

    public void GameOver(string _reason)
    {
        m_gameOverReasonText.text = _reason;
        Show(UIState.GameOver);
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

        m_waveSlider.value = wave.Progress01;
    }

    public void DisableLoadingScreen()
    {
        m_fakeLoadingScreen.SetActive(false);
        MusicManager.Instance.StopAllMusic();
        MusicManager.Instance.PlayMainMenuMusic();
    }

    public void UpdateScore()
    {
        if(m_scoresUI.Length == 0) return;

        UpdateCurrentScoresUI();
        UpdateBestScoresUI();
    }

    private void UpdateCurrentScoresUI()
    {
        foreach (TMP_Text scoreText in m_scoresUI)
        {
            if (scoreText == null)
            {
                Debug.LogError("scoresUI contains a null reference!");
                continue;
            }

            scoreText.text = ScoreManager.Instance.Score.ToString();
        }
    }


    private void UpdateBestScoresUI()
    {
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        int currentScore = ScoreManager.Instance.Score;

        if (currentScore > highscore)
        {
            highscore = currentScore;
            PlayerPrefs.SetInt("HighScore", highscore);
        }

        foreach (TMP_Text scoreText in m_bestScoresUI)
        {
            scoreText.text = highscore.ToString();
        }
    }
    public void UpdateLivesUI()
    {
        int l = GameManager.Instance.GetPlayerHealth().GetCurrentHealthValue();

        Color c = l <= 1 ? Color.red : Color.white;
        foreach (TMP_Text liveText in m_livesUI)
        {
            liveText.text = l.ToString();
        }
    }
}