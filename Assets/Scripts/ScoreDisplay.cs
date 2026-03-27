using UnityEngine;
using TMPro;

/// <summary>
/// Отображение счёта на экране.
/// </summary>
public class ScoreDisplay : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    private void Update()
    {
        if (_gameManager != null && _scoreText != null)
        {
            _scoreText.text = _gameManager.CurrentScore.ToString();
        }

        if (_highScoreText != null)
        {
            _highScoreText.text = $"Рекорд: {PlayerPrefs.GetInt("HighScore", 0)}";
        }
    }
}
