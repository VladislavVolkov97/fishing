using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Настройки")]
    public float mouseSensitivity = 2f;  // Чувствительность (настрой в Inspector)
    public Transform playerBody;         // Перетащи PlayerBody в Inspector
    public float xClamp = 90f;           // Лимит взгляда вверх/вниз

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Запереть курсор
        Cursor.visible = false;
    }

    void Update()
    {
        // Input (Legacy или Input System)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * 100f;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * 100f;

        // Вертикаль: только Head (X-ось)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Горизонталь: Body + Head (Y-ось, поворот с телом)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}