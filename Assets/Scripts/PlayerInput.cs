using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] InputActionReference moveInput;
    [SerializeField] private float moveSpeed = 2f;
    private CharacterController characterController;
    private void OnEnable()
    {
        moveInput.action.performed += MoveInputPerformed;
    }
   

    private void MoveInputPerformed(InputAction.CallbackContext obj)
    {
        

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var xInput = Input.GetAxis("Horizontal");
        var zInput = Input.GetAxis("Vertical");
        Vector2 input = moveInput.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        //transform.position += move * Time.deltaTime * moveSpeed;
        characterController.Move(move * Time.deltaTime * moveSpeed);

        if (move != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(move);

    }

    private void OnDisable()
    {
        moveInput.action.performed -= MoveInputPerformed;

    }
}
