using UnityEngine;

/// <summary>
/// Триггер для подсчёта очков при прохождении трубы.
/// Вешается на невидимый объект между верхней и нижней трубой.
/// Двигается влево вместе с трубами.
/// </summary>
public class ScoreTrigger : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    
    private GameManager _gameManager;
    private bool _scored = false;
    private ObjectPool<ScoreTrigger> _pool;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        
        if (_gameManager == null)
        {
            Debug.LogError("[ScoreTrigger] GameManager не найден в сцене!");
        }
    }

    /// <summary>
    /// Инициализация пулом и скоростью.
    /// </summary>
    public void Initialize(ObjectPool<ScoreTrigger> pool, float moveSpeed)
    {
        _pool = pool;
        _moveSpeed = moveSpeed;
    }

    private void Update()
    {
        // Движение влево вместе с трубами
        transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);

        // Возврат в пул при выходе за экран
        if (transform.position.x < -15f)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_scored) return;

        if (collision.CompareTag("Player"))
        {
            _scored = true;
            Debug.Log("[ScoreTrigger] Игрок прошёл трубу! +1 очко");
            _gameManager?.AddScore();
        }
    }

    /// <summary>
    /// Сброс для возврата в пул.
    /// </summary>
    public void ResetTrigger()
    {
        _scored = false;
    }

    public void ReturnToPool()
    {
        if (_pool != null)
        {
            _pool.Return(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
