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
    public Action shootAction;

    private void OnEnable()
    {
        shootInput.action.performed += ShootAction_performed;
    }

    private void ShootAction_performed(InputAction.CallbackContext obj)
    {
        shootAction.Invoke();
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
        shootInput.action.performed -= ShootAction_performed;

    }
}
