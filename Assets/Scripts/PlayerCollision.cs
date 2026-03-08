using UnityEngine;

/// <summary>
/// Обработка столкновений игрока.
/// При ударе о трубу или землю — Game Over.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PlayerCollision : MonoBehaviour
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Столкновение с трубой или землёй
        if (collision.gameObject.CompareTag("Obstacle") || 
            collision.gameObject.CompareTag("Ground"))
        {
            _gameManager?.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Триггер смерти (например, пол)
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            _gameManager?.GameOver();
        }
    }
}
