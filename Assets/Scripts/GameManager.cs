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
    [SerializeField] private MenuManager _menuManager;

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
    /// Начало игры (вызывается меню).
    /// </summary>
    public void StartGame()
    {
        _isGameRunning = true;
        _currentScore = 0;
        _pipeSpawner?.ResetSpawner();
        _player?.ResetState();
    }

    /// <summary>
    /// Остановка спавна труб (для меню).
    /// </summary>
    public void StopSpawning()
    {
        _pipeSpawner?.StopSpawning();
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

        // Показываем Game Over меню
        if (_menuManager != null)
        {
            _menuManager.ShowGameOver();
        }
    }

    /// <summary>
    /// Увеличение счёта.
    /// </summary>
    public void AddScore()
    {
        if (!_isGameRunning)
        {
            Debug.LogWarning("[GameManager] AddScore вызван, но игра не запущена!");
            return;
        }

        _currentScore++;
        Debug.Log($"[GameManager] Score: {_currentScore}");
    }

    /// <summary>
    /// Рестарт сцены.
    /// </summary>
    public void Restart()
    {
        Debug.Log("[GameManager] Restart() вызван, перезагружаю сцену...");
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
