using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 input;
    private PlayerInput playerInput;
    private Rigidbody rb;
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = (transform.forward * input.y + transform.right * input.x) * speed;
    }

    public void Move(InputAction.CallbackContext context){
        if (context.performed)
        {
            Debug.Log("wosh");
        }
    }

    private void GetInput()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
    }
}
