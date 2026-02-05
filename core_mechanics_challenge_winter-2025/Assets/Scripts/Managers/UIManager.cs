using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

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

    [Header("UI Elements")] [SerializeField]
    private TMP_Text m_gameOverReasonText;

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

    public void GameOver(string _reason)
    {
        m_gameOverReasonText.text = _reason;
        Show(UIState.GameOver);
    }

    public void Restart()
    {
        // GameManager.Instance.RestartGame();
    }

    // public void SelectUpgrade(UpgradeType type)
    // {
    //     UpgradeFactory.Apply(
    //         type,
    //         GameManager.Instance.PlayerStats,
    //         GameManager.Instance.PlayerHealth,
    //         GameManager.Instance.BaseHealth
    //     );
    //
    //     GameManager.Instance.RebuildWeapons();
    //     UIManager.Instance.Show(UIState.HUD);
    // }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Show(UIState.Pause);
            GameManager.Instance.TogglePause();
        }
    }

    public void DisableLoadingScreen()
    {
        m_fakeLoadingScreen.SetActive(false);
        MusicManager.Instance.StopAllMusic();
        MusicManager.Instance.PlayMainMenuMusic();
    }
}