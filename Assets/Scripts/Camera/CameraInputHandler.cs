using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference lookAction; // ← перетащи "Look" из Input Actions

    private CinemachineFreeLook freeLookCamera;

    private void Awake()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        if (freeLookCamera == null)
        {
            Debug.LogError("CinemachineFreeLook не найден!");
        }
    }

    private void OnEnable()
    {
        if (lookAction != null && lookAction.action != null)
        {
            lookAction.action.performed += OnLookPerformed;
            lookAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (lookAction != null && lookAction.action != null)
        {
            lookAction.action.performed -= OnLookPerformed;
            lookAction.action.Disable();
        }
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();

        // Для мыши: умножаем на чувствительность и Time.deltaTime
        // Для геймпада: уже нормализовано, можно меньше множитель
        float mouseSensitivity = 0.1f; // подстрой под себя (0.05–0.3)

        freeLookCamera.m_XAxis.Value += lookInput.x * mouseSensitivity;
        freeLookCamera.m_YAxis.Value += lookInput.y * mouseSensitivity * -1f; // инверсия Y (стандарт)
    }
}