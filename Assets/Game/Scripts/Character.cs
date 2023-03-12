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

    // Health
    private Health _health;

    // Damage Caster
    private DamageCaster _damageCaster;

    // State Machine
    public enum CharacterState 
    {
        Normal, Attacking
    }
    public CharacterState CurrentState;
    public float MoveSpeed = 5f;
    public float Gravity = -9.8f;

    private CharacterController _characterController;
    private Vector3 _movementVelocity;
    private PlayerInput _playerInput;
    private float _verticalVelocity;
    private Animator _animator;
    private string _playerTag = "Player";

    // Player slides
    private float attackStartTime;
    public float AttackSlideDuration = 0.4f;
    public float AttackSlideSpeed = 0.06f;

    // Update is called once per frame
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _health = GetComponent<Health>();
        _damageCaster = GetComponentInChildren<DamageCaster>();
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
            SwitchStateTo(CharacterState.Attacking);
        }
    }

    private void FixedUpdate()
    {
        switch(CurrentState) 
        {
            case CharacterState.Normal:
                if (IsPlayer && _playerInput.MouseButtonDown && _characterController.isGrounded)
                {
                    SwitchStateTo(CharacterState.Attacking);
                    return;
                }
                CalculateMovement();
                break;
            case CharacterState.Attacking:
                if (IsPlayer)
                {
                    _movementVelocity = Vector3.zero;
                    if (Time.time < attackStartTime + AttackSlideDuration)
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / AttackSlideDuration;
                        _movementVelocity = Vector3.Lerp(transform.forward * AttackSlideSpeed, Vector3.zero, lerpTime);
                    }
                }
                else 
                {
                    transform.LookAt(_target.position);
                }
                break;
        }
        MovePlayer();   
    }

    private void MovePlayer()
    {
        if (!IsPlayer) 
        {
            return;
        }
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

    private void CalculateMovement()
    {
        if (IsPlayer)
        {
            CalculatePlayerMovement();
        }
        else
        {
            CalculateEnemyMovement();
        } 
    }

    private void SwitchStateTo(CharacterState newState)
    {
        // Clear Cache
        if (IsPlayer) { _playerInput.MouseButtonDown = false; }

        // Exiting state
        switch(CurrentState)
        {
            case CharacterState.Normal:

                break;
            case CharacterState.Attacking:

                break;
        }

        // Entering state
        switch(newState)
        {
            case CharacterState.Normal:

                break;
            case CharacterState.Attacking:
                _animator.SetTrigger("Attack");
                if (IsPlayer)
                {
                    attackStartTime = Time.time;
                }
                break;
        }

        // Switch State
        CurrentState = newState;

        print($"Switched to {CurrentState}");
    }

    public void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void ApplyDamage(int damage, Vector3 attachPosition = new Vector3())
    {
        _health?.ApplyDamage(damage);
    }

    public void EnableDamageCaster()
    {
        _damageCaster.EnableDamageCaster();
    }

    public void DisableDamageCaster()
    {
        _damageCaster.DisableDamageCaster();
    }

}
