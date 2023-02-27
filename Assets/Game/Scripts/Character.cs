using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    // Enemy
    public bool IsPlayer = false;
    private NavMeshAgent _navMeshAgent;
    private Transform _target;


    public float MoveSpeed = 5f;
    public float Gravity = -9.8f;

    private CharacterController _characterController;
    private Vector3 _movementVelocity;
    private PlayerInput _playerInput;
    private float _verticalVelocity;
    private Animator _animator;
    private string _playerTag = "Player";

    // Update is called once per frame
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        
        if (!IsPlayer)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _target = GameObject.FindWithTag(_playerTag).transform;
            _navMeshAgent.speed = MoveSpeed;
        }
        else 
        {
            _playerInput = GetComponent<PlayerInput>();
        }
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

    private void CalculateEnemyMovement()
    {
        if (Vector3.Distance(_target.position, transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(_target.position);
            _animator.SetFloat("Speed", 0.2f);
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetFloat("Speed", 0f);
        }
    }

    private void FixedUpdate()
    {
        if (IsPlayer)
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
        else
        {
            CalculateEnemyMovement();
        }
       
    }
}
