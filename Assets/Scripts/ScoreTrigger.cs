using UnityEngine;

/// <summary>
/// Триггер для подсчёта очков при прохождении трубы.
/// Вешается на невидимый объект между трубами.
/// </summary>
public class ScoreTrigger : MonoBehaviour
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _gameManager?.AddScore();
        }
    }
}
