using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using Dan.Models;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    private string m_publicKey = LeaderboardKey.LEADERBOARD_KEY;

    [SerializeField] private Transform m_leaderboardContentTF;
    [SerializeField] private GameObject m_leaderboardEntryPrefab;
    [SerializeField] private GameObject m_leaderboardPanel, m_leaderboardSubmissionPanel, m_leaderboardSubmissionText;
    [SerializeField] private TMP_InputField m_playerNameInputField;

    [SerializeField] private int m_maxEntryCount = 30;

    private List<LeaderboardEntry> m_currentEntries;

    private void Start()
    {
        m_currentEntries = new List<LeaderboardEntry>();
        ResetLeaderboardUI();
        GetLeaderBoard();
    }

    public void EnableSubmissionPanel(bool enabled)
    {
        m_leaderboardSubmissionPanel.SetActive(enabled);
    }

    public void EnableLeaderboardPanel(bool enabled)
    {
        m_leaderboardPanel.SetActive(enabled);
    }

    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(m_publicKey, OnLeaderboardLoaded, ErrorCallback);
    }

    public void UploadNewEntry()
    {
        if (m_playerNameInputField.text == string.Empty)
        { return; }

        string username = m_playerNameInputField.text;
        int score = ScoreManager.Instance.Score;

        SetEntry(username, score);

    }

    private void SetEntry(string _username, int _score)
    {
        LeaderboardCreator.UploadNewEntry(m_publicKey, _username, _score, ((msg) =>
        {
            m_leaderboardSubmissionPanel.SetActive(false);
            m_leaderboardPanel.SetActive(true);
            m_leaderboardSubmissionText.SetActive(true);
            GetLeaderBoard();
        }));
    }

    private void OnLeaderboardLoaded(Entry[] entries)
    {
        foreach (Transform _childTF in m_leaderboardContentTF)
        {
            m_currentEntries = new List<LeaderboardEntry>();
            Destroy(_childTF.gameObject);
        }

        if (entries.Length < m_maxEntryCount)
        {
            foreach (var t in entries)
            {
                LeaderboardEntry newEntry = Instantiate(m_leaderboardEntryPrefab, m_leaderboardContentTF)
                                               .GetComponent<LeaderboardEntry>();

                newEntry.InitializeEntry(t.Rank, t.Username, t.Score);
                m_currentEntries.Add(newEntry);
            }
        }
        else
        {
            for (int i = 0; i < m_maxEntryCount; i++)
            {
                Entry t = entries[i];
                LeaderboardEntry newEntry = Instantiate(m_leaderboardEntryPrefab, m_leaderboardContentTF)
                                               .GetComponent<LeaderboardEntry>();

                newEntry.InitializeEntry(t.Rank, t.Username, t.Score);
                m_currentEntries.Add(newEntry);
            }

            GetPersonalEntry();
        }
    }

    public void GetPersonalEntry()
    {
        LeaderboardCreator.GetPersonalEntry(m_publicKey, OnPersonalEntryLoaded, ErrorCallback);
    }

    private void OnPersonalEntryLoaded(Entry _entry)
    {
        LeaderboardEntry newEntry = Instantiate(m_leaderboardEntryPrefab, m_leaderboardContentTF)
                                               .GetComponent<LeaderboardEntry>();

        newEntry.InitializeEntry(_entry.Rank, _entry.Username, _entry.Score);
    }

    private void ErrorCallback(string error)
    {
        Debug.Log(error);
    }

    private void ResetLeaderboardUI()
    {
        m_leaderboardSubmissionPanel.SetActive(true);
        m_leaderboardPanel.SetActive(false);
        m_leaderboardSubmissionText.SetActive(false);
    }
}