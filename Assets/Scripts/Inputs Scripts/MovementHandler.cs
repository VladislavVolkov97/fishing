using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    private Vector2 _currentMoveInput = Vector2.zero;
    private bool _jumpPressed;
    private bool _sprintPressed;

    private CharacterController _controller;
    [SerializeField] private Camera mainCamera;  // ← лучше перетащить вручную

    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
   // public float jumpForce = 5f;
    public float jumpForce = 20f;
    private Vector3 _velocity;



    [Header("Падение с высоты")]
    public float fallGravityMultiplier = 2.5f;      // 2–4 — падение в 2–4 раза быстрее подъёма
    public float maxFallSpeed = 30f;                // максимальная скорость падения (юниты/сек), 20–50 — реалистично

    void Awake()
    {
        _controller = GetComponent<CharacterController>();

        // Вариант 1 — самый надёжный (рекомендую)
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Вариант 2 — если не хочешь перетаскивать
        // mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera не найдена! Убедись, что у камеры стоит тег MainCamera.");
        }
    }

    void Update()
    {
        // Гравитация
        /* if (IsGroundedCustom() && _velocity.y < 0)
             _velocity.y = -3.5f;*/
        bool grounded = _controller.isGrounded;  // или твой custom grounded

        if (grounded && _velocity.y < 0)
        {
            _velocity.y = -3.5f;  // сильное прижатие (было -2f — увеличь)
        }
        else
        {
            float currentGravity = Physics.gravity.y;

            if (_velocity.y < 0)  // падает — усиливаем гравитацию
            {
                currentGravity *= fallGravityMultiplier;  // например -9.81 * 2.5 = -24.525
            }

            _velocity.y += currentGravity * Time.deltaTime;

            // Ограничиваем максимальную скорость падения (terminal velocity)
            if (_velocity.y < -maxFallSpeed)
            {
                _velocity.y = -maxFallSpeed;
            }
        }

        //_velocity.y += Physics.gravity.y * Time.deltaTime;
        _velocity.y += -20f * Time.deltaTime;  // или -30f, -40f — подбери "на глаз"


        // Движение относительно камеры
        if (mainCamera != null)
        {
            Vector3 camForward = mainCamera.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = mainCamera.transform.right;
            camRight.y = 0f;
            camRight.Normalize();

            Vector3 move = (camRight * _currentMoveInput.x + camForward * _currentMoveInput.y).normalized;
            float currentSpeed = _sprintPressed ? sprintSpeed : walkSpeed;

            _controller.Move(move * currentSpeed * Time.deltaTime);

            // Поворот тела по горизонтали камеры
            if (camForward.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(camForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
            }
        }

        if (_jumpPressed)
        {
            Debug.Log($"[Jump] Флаг true в начале Update! isGrounded = {IsGroundedCustom()}");
        }

        // Прыжок
        if (_jumpPressed && IsGroundedCustom())
        {
            Debug.Log("ПРЫЖОК ВЫПОЛНЕН!");
            _velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            _jumpPressed = false;
        }
        else if (_jumpPressed)
        {
            Debug.LogWarning("Jump pressed, но НЕ на земле → пропускаем");
        }

        _controller.Move(_velocity * Time.deltaTime);
    }


    [Header("Ground Check")]
    public float groundCheckRadius = 0.2f;  // радиус сферы
    public float groundCheckDistance = 0.1f;  // расстояние вниз
    public LayerMask groundMask = 1;  // Layer "Ground" или Default

    private bool IsGroundedCustom()
    {
        Vector3 checkPos = transform.position - Vector3.up * groundCheckDistance;  // центр чуть ниже ног
        return Physics.CheckSphere(checkPos, groundCheckRadius, groundMask);
    }
    // Методы для ввода (без изменений)
    public void SetMovementInput(Vector2 value) => _currentMoveInput = value;
    public void SetJumpInput(bool value) => _jumpPressed = value;
    public void SetSprintInput(bool value) => _sprintPressed = value;

    public Vector2 GetMoveInput() => _currentMoveInput;
    public bool IsSprinting() => _sprintPressed;
}