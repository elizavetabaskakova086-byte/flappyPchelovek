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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Столкновение с трубой или землёй
        if (collision.CompareTag("Obstacle") || 
            collision.CompareTag("Ground"))
        {
            _gameManager?.GameOver();
        }
    }
}
