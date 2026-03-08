using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Центральный менеджер игры.
/// Управляет состоянием игры, счётом, рестартом.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private PlayerController _player;
    [SerializeField] private PipeSpawner _pipeSpawner;

    [Header("Настройки счёта")]
    [SerializeField] private float _scorePointX = 0f; // Позиция для подсчёта очков

    private int _currentScore;
    private int _highScore;
    private bool _isGameRunning = false;

    private void Awake()
    {
        // Загрузка рекорда
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void Start()
    {
        StartGame();
    }

    /// <summary>
    /// Начало игры.
    /// </summary>
    public void StartGame()
    {
        _isGameRunning = true;
        _currentScore = 0;
        _pipeSpawner?.ResetSpawner();
        _player?.ResetState();
    }

    /// <summary>
    /// Конец игры (смерть игрока).
    /// </summary>
    public void GameOver()
    {
        _isGameRunning = false;
        _player?.SetDead();
        _pipeSpawner?.StopSpawning();

        // Сохранение рекорда
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            PlayerPrefs.SetInt("HighScore", _highScore);
            PlayerPrefs.Save();
        }

        Debug.Log($"Game Over! Score: {_currentScore}, High Score: {_highScore}");
    }

    /// <summary>
    /// Увеличение счёта.
    /// </summary>
    public void AddScore()
    {
        if (!_isGameRunning) return;

        _currentScore++;
        Debug.Log($"Score: {_currentScore}");
    }

    /// <summary>
    /// Рестарт сцены.
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Текущий счёт.
    /// </summary>
    public int CurrentScore => _currentScore;

    /// <summary>
    /// Рекорд.
    /// </summary>
    public int HighScore => _highScore;

    /// <summary>
    /// Игра активна.
    /// </summary>
    public bool IsGameRunning => _isGameRunning;
}
