using UnityEngine;
using UnityEngine.InputSystem;     // обязательно

public class MyInputSystem : MonoBehaviour
{
    [Header("Input References — перетащи из инспектора")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;     // опционально

    [Header("Handler — твой компонент с логикой движения")]
    [SerializeField] private MovementHandler movementHandler;       // ← перетащи сюда

    private void OnEnable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.performed += OnMovePerformed;
            moveAction.action.canceled += OnMoveCanceled;
            moveAction.action.Enable();
        }

        if (jumpAction != null && jumpAction.action != null)
        {
            jumpAction.action.performed += OnJumpPerformed;
            // canceled для прыжка обычно не нужен (если Button-тип)
            jumpAction.action.Enable();
        }

        if (sprintAction != null && sprintAction.action != null)
        {
            sprintAction.action.performed += OnSprintPerformed;
            sprintAction.action.canceled += OnSprintCanceled;
            sprintAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.performed -= OnMovePerformed;
            moveAction.action.canceled -= OnMoveCanceled;
            moveAction.action.Disable();
        }

        if (jumpAction != null && jumpAction.action != null)
        {
            jumpAction.action.performed -= OnJumpPerformed;
            jumpAction.action.Disable();
        }

        if (sprintAction != null && sprintAction.action != null)
        {
            sprintAction.action.performed -= OnSprintPerformed;
            sprintAction.action.canceled -= OnSprintCanceled;
            sprintAction.action.Disable();
        }
    }

    // ──────────────────────────────────────────────
    // Методы для движения (самые важные)
    // ──────────────────────────────────────────────

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        movementHandler.SetMovementInput(input);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movementHandler.SetMovementInput(Vector2.zero);
    }

    // ──────────────────────────────────────────────
    // Прыжок (обычно только performed, т.к. Button)
    // ──────────────────────────────────────────────

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        movementHandler.SetJumpInput(true);
    }

    // Если прыжок держится (Hold-тип), можно добавить:
    // private void OnJumpCanceled(InputAction.CallbackContext context)
    // {
    //     movementHandler.SetJumpInput(false);
    // }

    // ──────────────────────────────────────────────
    // Спринт (если есть)
    // ──────────────────────────────────────────────

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        movementHandler.SetSprintInput(true);
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        movementHandler.SetSprintInput(false);
    }

    // Опционально: если хочешь ловить любые изменения без performed/canceled
    // (редко нужно для движения)
    // private void OnMoveValueChanged(InputAction.CallbackContext context)
    // {
    //     movementHandler.SetMovementInput(context.ReadValue<Vector2>());
    // }
}