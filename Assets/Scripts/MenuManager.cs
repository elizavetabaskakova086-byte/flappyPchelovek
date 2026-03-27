using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Менеджер UI меню на UI Toolkit.
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private UIDocument _mainMenuDocument;
    [SerializeField] private UIDocument _gameOverDocument;
    [SerializeField] private TMPro.TextMeshProUGUI _scoreText;

    [Header("Фон меню")]
    [SerializeField] private Sprite _menuBackgroundSprite;

    private Button _startButton;
    private Button _quitButton;
    private Button _restartButton;
    private Button _menuButton;
    private Label _finalScoreLabel;
    private Label _highScoreLabel;
    private VisualElement _menuBackground;
    private bool _gameOverButtonsInitialized = false;

    private void OnEnable()
    {
        if (_mainMenuDocument != null)
        {
            var root = _mainMenuDocument.rootVisualElement;
            if (root != null)
            {
                _startButton = root.Q<Button>("StartButton");
                _quitButton = root.Q<Button>("QuitButton");

                if (_startButton != null)
                    _startButton.clicked += StartGame;

                if (_quitButton != null)
                    _quitButton.clicked += QuitGame;
            }
        }
    }

    private void OnDisable()
    {
        if (_startButton != null)
            _startButton.clicked -= StartGame;

        if (_quitButton != null)
            _quitButton.clicked -= QuitGame;

        if (_restartButton != null)
            _restartButton.clicked -= RestartGame;

        if (_menuButton != null)
            _menuButton.clicked -= ShowMainMenu;
    }

    private void InitializeGameOverButtons()
    {
        if (_gameOverButtonsInitialized) return;

        if (_gameOverDocument != null)
        {
            var root = _gameOverDocument.rootVisualElement;
            if (root != null)
            {
                _restartButton = root.Q<Button>("RestartButton");
                _menuButton = root.Q<Button>("MenuButton");
                _finalScoreLabel = root.Q<Label>("FinalScoreLabel");
                _highScoreLabel = root.Q<Label>("HighScoreLabel");

                if (_restartButton != null)
                    _restartButton.clicked += RestartGame;

                if (_menuButton != null)
                    _menuButton.clicked += ShowMainMenu;

                _gameOverButtonsInitialized = true;
            }
        }
    }

    private void Update()
    {
        // Обновление счёта во время игры
        if (_scoreText != null && _gameManager != null && _gameManager.IsGameRunning)
        {
            _scoreText.text = _gameManager.CurrentScore.ToString();
        }
    }

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 0;

        if (_mainMenuDocument != null)
        {
            _mainMenuDocument.enabled = true;

            // Устанавливаем фон
            if (_menuBackgroundSprite != null)
            {
                var root = _mainMenuDocument.rootVisualElement;
                _menuBackground = root.Q<VisualElement>("Background");
                if (_menuBackground != null)
                {
                    var bgTexture = new StyleBackground(_menuBackgroundSprite.texture);
                    _menuBackground.style.backgroundImage = bgTexture;
                    _menuBackground.style.opacity = 1;
                }
            }
        }

        if (_gameOverDocument != null)
            _gameOverDocument.enabled = false;

        // Скрываем счёт
        if (_scoreText != null)
            _scoreText.enabled = false;

        // Остановить спавн труб
        _gameManager?.StopSpawning();
    }

    public void StartGame()
    {
        Time.timeScale = 1;

        if (_mainMenuDocument != null)
            _mainMenuDocument.enabled = false;

        // Показываем счёт
        if (_scoreText != null)
        {
            _scoreText.enabled = true;
            _scoreText.text = "0";
        }

        _gameManager?.StartGame();
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0;

        if (_mainMenuDocument != null)
            _mainMenuDocument.enabled = false;

        if (_gameOverDocument != null)
        {
            _gameOverDocument.enabled = true;
            
            // Инициализируем кнопки при первом показе
            InitializeGameOverButtons();

            // Обновляем счёт
            if (_finalScoreLabel != null && _gameManager != null)
                _finalScoreLabel.text = $"Очки: {_gameManager.CurrentScore}";

            if (_highScoreLabel != null)
                _highScoreLabel.text = $"Рекорд: {PlayerPrefs.GetInt("HighScore", 0)}";
        }
    }

    public void RestartGame()
    {
        Debug.Log("[MenuManager] RestartGame нажата!");
        
        Time.timeScale = 1;

        if (_gameOverDocument != null)
        {
            _gameOverDocument.enabled = false;
            Debug.Log("[MenuManager] GameOverDocument выключен");
        }

        // Сброс и показ счёта
        if (_scoreText != null)
        {
            _scoreText.text = "0";
        }

        Debug.Log("[MenuManager] Вызываю GameManager.Restart()");
        _gameManager?.Restart();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
