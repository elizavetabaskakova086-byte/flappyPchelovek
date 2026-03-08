using UnityEngine;

/// <summary>
/// Препятствие (труба) для FlappyPchelovek.
/// Двигается влево, уничтожается при выходе за экран.
/// </summary>
public class Pipe : MonoBehaviour
{
    private float _moveSpeed = 3f;
    private ObjectPool<Pipe> _pool;

    /// <summary>
    /// Инициализация пулом и скоростью.
    /// </summary>
    public void Initialize(ObjectPool<Pipe> pool, float moveSpeed)
    {
        _pool = pool;
        _moveSpeed = moveSpeed;
    }

    private void Update()
    {
        // Движение влево
        transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);

        // Возврат в пул при выходе за экран
        if (transform.position.x < -15f)
        {
            ReturnToPool();
        }
    }

    private void OnBecameInvisible()
    {
        // Дополнительная страховка
        ReturnToPool();
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
