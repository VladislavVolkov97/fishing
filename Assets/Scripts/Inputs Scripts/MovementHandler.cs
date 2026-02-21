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
    public float jumpForce = 5f;
    private Vector3 _velocity;

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
        if (_controller.isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _velocity.y += Physics.gravity.y * Time.deltaTime;

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

        // Прыжок
        if (_jumpPressed && _controller.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            _jumpPressed = false;
        }

        _controller.Move(_velocity * Time.deltaTime);
    }

    // Методы для ввода (без изменений)
    public void SetMovementInput(Vector2 value) => _currentMoveInput = value;
    public void SetJumpInput(bool value) => _jumpPressed = value;
    public void SetSprintInput(bool value) => _sprintPressed = value;

    public Vector2 GetMoveInput() => _currentMoveInput;
    public bool IsSprinting() => _sprintPressed;
}