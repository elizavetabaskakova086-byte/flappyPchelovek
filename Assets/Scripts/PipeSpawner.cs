using UnityEngine;

/// <summary>
/// Спавнер труб с процедурной генерацией.
/// Создаёт пары труб (верхняя + нижняя) с случайным зазором.
/// </summary>
public class PipeSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    [SerializeField] private Pipe _pipePrefab;
    [SerializeField] private float _spawnInterval = 1.5f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _minGapHeight = 2.5f;
    [SerializeField] private float _maxGapHeight = 4f;
    [SerializeField] private float _spawnRangeY = 3f;

    [Header("Пул")]
    [SerializeField] private int _poolSize = 10;

    private ObjectPool<Pipe> _pool;
    private float _spawnTimer;
    private bool _isSpawning = true;

    private void Start()
    {
        // Инициализация пула
        _pool = new ObjectPool<Pipe>(_pipePrefab, _poolSize, transform);
    }

    private void Update()
    {
        if (!_isSpawning) return;

        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnPipePair();
        }
    }

    /// <summary>
    /// Спавн пары труб (верхняя и нижняя).
    /// </summary>
    private void SpawnPipePair()
    {
        // Случайная позиция по Y для зазора
        float gapCenterY = Random.Range(-_spawnRangeY, _spawnRangeY);
        float gapHeight = Random.Range(_minGapHeight, _maxGapHeight);

        // Позиции труб
        float spawnX = transform.position.x;
        float topPipeY = gapCenterY + gapHeight / 2f;
        float bottomPipeY = gapCenterY - gapHeight / 2f;

        // Спавн верхней трубы
        Pipe topPipe = _pool.Get();
        topPipe.transform.position = new Vector3(spawnX, topPipeY, 0);
        topPipe.Initialize(_pool, _moveSpeed);

        // Спавн нижней трубы
        Pipe bottomPipe = _pool.Get();
        bottomPipe.transform.position = new Vector3(spawnX, bottomPipeY, 0);
        bottomPipe.Initialize(_pool, _moveSpeed);
    }

    /// <summary>
    /// Остановка спавна (для паузы или game over).
    /// </summary>
    public void StopSpawning()
    {
        _isSpawning = false;
    }

    /// <summary>
    /// Запуск спавна и очистка всех труб.
    /// </summary>
    public void ResetSpawner()
    {
        _isSpawning = true;
        _spawnTimer = _spawnInterval;
    }
}
