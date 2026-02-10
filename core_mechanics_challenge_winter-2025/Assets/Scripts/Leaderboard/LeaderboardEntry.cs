using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text m_entryNumberText, m_usernameText, m_scoreText;

    public void InitializeEntry(int _number, string _name, int _score)
    {
        m_entryNumberText.text = _number.ToString();
        m_usernameText.text = _name;
        m_scoreText.text = _score.ToString();
    }
}
