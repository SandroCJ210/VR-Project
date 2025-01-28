using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 _input;
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move(){
        _rigidbody.linearVelocity = (transform.forward * _input.y + transform.right * _input.x) * speed;
    }

    private void GetInput()
    {
        _input = _playerInput.actions["Move"].ReadValue<Vector2>();
    }
}
