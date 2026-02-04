using UnityEngine;
using System.Collections.Generic;

public enum UIState
{
    MainMenu,
    HUD,
    Pause,
    Upgrade,
    GameOver
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_hud;
    [SerializeField] private GameObject m_pause;
    [SerializeField] private GameObject m_upgrade;
    [SerializeField] private GameObject m_gameOver;

    private Dictionary<UIState, GameObject> m_panels;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        m_panels = new Dictionary<UIState, GameObject>
        {
            { UIState.MainMenu, m_mainMenu },
            { UIState.HUD, m_hud },
            { UIState.Pause, m_pause },
            { UIState.Upgrade, m_upgrade },
            { UIState.GameOver, m_gameOver }
        };

        Show(UIState.MainMenu);
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

    public void GameOver()
    {
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
        }
    }
}