using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float Gravity = -9.8f;

    private CharacterController _characterController;
    private Vector3 _movementVelocity;
    private PlayerInput _playerInput;
    private float _verticalVelocity;
    private Animator _animator;

    // Update is called once per frame
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity.Set(_playerInput.HorizontalInput, 0f, _playerInput.VerticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;

        _animator.SetFloat("Speed", _movementVelocity.magnitude);

        _movementVelocity *= MoveSpeed * Time.deltaTime;

        if (_movementVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
        }
        _animator.SetBool("AirBorne", !_characterController.isGrounded);
    }

    private void FixedUpdate()
    {
        CalculatePlayerMovement();
        if (!_characterController.isGrounded)
        {
            _verticalVelocity = Gravity;
        }
        else
        {
            _verticalVelocity = Gravity * 0.3f;
        }
        _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
        _characterController.Move(_movementVelocity);
    }
}
