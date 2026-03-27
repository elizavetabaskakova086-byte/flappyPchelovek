using UnityEngine;

/// <summary>
/// Спавнер труб с процедурной генерацией.
/// Создаёт пары труб (верхняя + нижняя) с случайным зазором.
/// </summary>
public class PipeSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    [SerializeField] private Pipe _pipePrefab;
    [SerializeField] private ScoreTrigger _scoreTriggerPrefab;
    [SerializeField] private float _spawnInterval = 1.5f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _minGapHeight = 2.5f;
    [SerializeField] private float _maxGapHeight = 4f;
    [SerializeField] private float _spawnRangeY = 3f;
    [SerializeField] private float _pipeHeight = 5f; // Высота спрайта трубы

    [Header("Пул")]
    [SerializeField] private int _poolSize = 10;

    private ObjectPool<Pipe> _pipePool;
    private ObjectPool<ScoreTrigger> _triggerPool;
    private float _spawnTimer;
    private bool _isSpawning = true;

    private void Start()
    {
        // Инициализация пулов
        _pipePool = new ObjectPool<Pipe>(_pipePrefab, _poolSize, transform);
        if (_scoreTriggerPrefab != null)
        {
            _triggerPool = new ObjectPool<ScoreTrigger>(_scoreTriggerPrefab, _poolSize, transform);
        }
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
    /// Спавн пары труб (верхняя и нижняя) + триггер счета.
    /// </summary>
    private void SpawnPipePair()
    {
        // Случайная позиция по Y для центра зазора
        float gapCenterY = Random.Range(-_spawnRangeY, _spawnRangeY);
        float gapHeight = Random.Range(_minGapHeight, _maxGapHeight);

        // Позиции центров труб
        float spawnX = transform.position.x;
        float halfGap = gapHeight / 2f;
        float halfPipe = _pipeHeight / 2f;

        // Верхняя труба: центр выше зазора на (половина зазора + половина трубы)
        float topPipeY = gapCenterY + halfGap + halfPipe;

        // Нижняя труба: центр ниже зазора на (половина зазора + половина трубы)
        float bottomPipeY = gapCenterY - halfGap - halfPipe;

        // Спавн верхней трубы (перевёрнута через scale)
        Pipe topPipe = _pipePool.Get();
        topPipe.transform.position = new Vector3(spawnX, topPipeY, 0);
        topPipe.transform.rotation = Quaternion.identity;
        topPipe.transform.localScale = new Vector3(1, -1, 1);
        topPipe.Initialize(_pipePool, _moveSpeed);

        // Спавн нижней трубы (обычная ориентация)
        Pipe bottomPipe = _pipePool.Get();
        bottomPipe.transform.position = new Vector3(spawnX, bottomPipeY, 0);
        bottomPipe.transform.rotation = Quaternion.identity;
        bottomPipe.transform.localScale = Vector3.one;
        bottomPipe.Initialize(_pipePool, _moveSpeed);

        // Спавн триггера счета между трубами
        if (_triggerPool != null)
        {
            ScoreTrigger trigger = _triggerPool.Get();
            trigger.transform.position = new Vector3(spawnX, gapCenterY, 0);
            trigger.transform.rotation = Quaternion.identity;
            trigger.transform.localScale = Vector3.one;
            trigger.Initialize(_triggerPool, _moveSpeed);
            trigger.ResetTrigger();
        }
    }

    /// <summary>
    /// Остановка спавна (для паузы или game over).
    /// </summary>
    public void StopSpawning()
    {
        _isSpawning = false;
    }

    /// <summary>
    /// Запуск спавна и сброс таймера.
    /// </summary>
    public void ResetSpawner()
    {
        _isSpawning = true;
        _spawnTimer = 0f;
    }

    /// <summary>
    /// Очистка всех труб (возврат в пул).
    /// </summary>
    public void ClearPipes()
    {
        _isSpawning = false;
    }
}
