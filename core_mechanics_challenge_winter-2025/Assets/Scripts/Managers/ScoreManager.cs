using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private string scorePrefab;
    
    public int Score { get; private set; }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += ResetScore;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= ResetScore;
    }

    public void AddScore(int _scoreToAdd)
    {
        Score += _scoreToAdd;
        UIManager.Instance.UpdateScore();
    }
    
    public void AddScore(int _scoreToAdd, Vector3 _position)
    {
        GameObject scoreObject = ObjectPoolManager.Instance.SpawnPooledObject(scorePrefab, _position, Quaternion.identity);
        TMP_Text t = scoreObject.GetComponentInChildren<TMP_Text>();
        t.text = _scoreToAdd.ToString();
        scoreObject.SetActive(true);
        
        AddScore(_scoreToAdd);
    }

    private void ResetScore()
    {
        Score = 0;
        UIManager.Instance.UpdateScore();
    }
}
