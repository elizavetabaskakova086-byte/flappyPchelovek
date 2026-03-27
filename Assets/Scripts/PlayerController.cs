using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Контроллер игрока для FlappyPchelovek.
/// Использует Rigidbody2D для физики и Input System для ввода.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Настройки прыжка")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _maxRotation = 90f;
    [SerializeField] private float _minRotation = -30f;
    [SerializeField] private float _deathY = -6f; // Позиция смерти при падении

    private Rigidbody2D _rb;
    private bool _isDead = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        // Отключаем вращение через физику, будем вращать вручную
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        if (_isDead) return;

        // Вращение игрока в зависимости от вертикальной скорости
        float targetRotation = Mathf.Clamp(_rb.linearVelocity.y * 10, _minRotation, _maxRotation);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, targetRotation, Time.deltaTime * _rotationSpeed));

        // Смерть при падении ниже экрана
        if (transform.position.y < _deathY)
        {
            SetDead();
            FindObjectOfType<GameManager>()?.GameOver();
        }
    }

    /// <summary>
    /// Вызывается Input System при нажатии кнопки прыжка.
    /// </summary>
    public void OnJump(InputValue value)
    {
        if (_isDead) return;

        if (value.isPressed)
        {
            Jump();
        }
    }

    /// <summary>
    /// Прыжок также работает по клику мыши или тапу.
    /// </summary>
    private void OnMouseDown()
    {
        if (_isDead) return;
        Jump();
    }

    private void Jump()
    {
        _rb.linearVelocity = new Vector2(0, _jumpForce);
    }

    /// <summary>
    /// Вызывается при столкновении для активации состояния смерти.
    /// </summary>
    public void SetDead()
    {
        _isDead = true;
        _rb.linearVelocity = Vector2.zero;
        // Поворачиваем игрока вниз
        transform.rotation = Quaternion.Euler(0, 0, _minRotation);
    }

    /// <summary>
    /// Сброс состояния для рестарта.
    /// </summary>
    public void ResetState()
    {
        _isDead = false;
        _rb.linearVelocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
    }
}
