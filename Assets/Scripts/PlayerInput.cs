using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference mouseInput;
    [SerializeField] InputActionReference shootInput;

    Vector2 moveVector;
    Vector2 mousePosition;

    public Action<Vector2> moveAction;
    public Action<Vector2> lookAction;
    public Action shootStartedAction;
    public Action shootCancelledAction;

    private void OnEnable()
    {
        shootInput.action.started += ShootAction_started;
        shootInput.action.canceled += ShootAction_canceled;
    }

    private void ShootAction_canceled(InputAction.CallbackContext obj)
    {
        shootCancelledAction.Invoke();
    }

    private void ShootAction_started(InputAction.CallbackContext obj)
    {
        shootStartedAction.Invoke();
    }

    private void Update()
    {
        moveVector = moveInput.action.ReadValue<Vector2>();
        moveAction.Invoke(moveVector);

        mousePosition = mouseInput.action.ReadValue<Vector2>();
        lookAction.Invoke(mousePosition);
    }

    private void OnDisable()
    {
        shootInput.action.started  -= ShootAction_started;
        shootInput.action.canceled -= ShootAction_canceled;

    }
}
