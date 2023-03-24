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

    // Coin
    public int Coin;

    // Heal Orb
    public GameObject ItemToDrop;

    // Damage Caster
    private DamageCaster _damageCaster;

    // State Machine
    public enum CharacterState 
    {
        Normal, Attacking, Dead, BeingHit
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
    private Vector3 impactOnCharacter;
    private float impactForce = 10f;

    public bool IsInvincible;
    public float InvincibleDuration = 2f;

    // Material animation
    private MaterialPropertyBlock _materialPropertyBloc;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    // Update is called once per frame
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _health = GetComponent<Health>();
        _damageCaster = GetComponentInChildren<DamageCaster>();
        _animator = GetComponent<Animator>();

        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBloc = new MaterialPropertyBlock();
        _skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBloc);
        
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
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:
                if (impactOnCharacter.magnitude > 0.2f)
                {
                    _movementVelocity = impactOnCharacter * Time.deltaTime;
                }
                impactOnCharacter = Vector3.Lerp(impactOnCharacter, Vector3.zero, Time.deltaTime * 5);
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
        _movementVelocity = Vector3.zero;
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

    public void SwitchStateTo(CharacterState newState)
    {
        // Clear Cache
        if (IsPlayer) { _playerInput.MouseButtonDown = false; }

        // Exiting state
        switch(CurrentState)
        {
            case CharacterState.Normal:

                break;
            case CharacterState.Attacking:
                if (_damageCaster != null)
                {
                    DisableDamageCaster();
                }
                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:
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
            case CharacterState.Dead:
                _characterController.enabled = false;
                _animator.SetTrigger("Dead");
                StartCoroutine(MaterialDissolve());
                break;
            case CharacterState.BeingHit:
                _animator.SetTrigger("BeingHit");
                if (IsPlayer)
                {
                    IsInvincible = true;
                    StartCoroutine(DelayCancelInvincible());
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

    public void BeingHitAnimationEnds()
    {
         SwitchStateTo(CharacterState.Normal);
    }

    public void ApplyDamage(int damage, Vector3 attachPosition = new Vector3())
    {
        if (IsInvincible)
        {
            return;
        }
        _health?.ApplyDamage(damage);

        if (!IsPlayer)
        {
            GetComponent<EnemyVFXManager>().PlayBeingHitVFX(attachPosition);
        }

        StartCoroutine(MaterialBlink());

        if (IsPlayer)
        {
            SwitchStateTo(CharacterState.BeingHit);
            AddImpact(attachPosition, impactForce);
        }
    }

    IEnumerator DelayCancelInvincible()
    {
        yield return new WaitForSeconds(InvincibleDuration);
        IsInvincible = false;
    }

    private void AddImpact(Vector3 attackerPosition, float force)
    {
        Vector3 impactDirection = transform.position - attackerPosition;
        impactDirection.Normalize();
        impactDirection.y = 0;
        impactOnCharacter = impactDirection * force;
    }

    public void EnableDamageCaster()
    {
        _damageCaster.EnableDamageCaster();
    }

    public void DisableDamageCaster()
    {
        _damageCaster.DisableDamageCaster();
    }

    IEnumerator MaterialBlink()
    {
        _materialPropertyBloc.SetFloat("_blink", 0.4f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBloc);

        yield return new WaitForSeconds(0.2f);

        _materialPropertyBloc.SetFloat("_blink", 0f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBloc);
    }

    IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2f);
        float dissolveTimeDuration = 2f;
        float currentDissolveTime = 0;
        float dissolveHightStart = 20f;
        float dissolveHightTarget = -10f;
        float dissolveHight;

        _materialPropertyBloc.SetFloat("_enableDissolve", 1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBloc);

        while (currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            dissolveHight = Mathf.Lerp(dissolveHightStart, dissolveHightTarget, currentDissolveTime / dissolveTimeDuration);
            _materialPropertyBloc.SetFloat("_dissolve_height", dissolveHight);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBloc);
            yield return null;
        }

        DropItem();
    }

    public void DropItem()
    {
        if (ItemToDrop == null) { return; }
        Instantiate(ItemToDrop, transform.position, Quaternion.identity);
    }

    public void PickUpItem(PickUp item)
    {
        switch(item.Type)
        {
            case PickUp.PickUpType.Heal:
                AddHealth(item.Value);
                break;
            case PickUp.PickUpType.Coin:
                AddCoin(item.Value);
                break;
        }
    }

    private void AddHealth(int health)
    {
        _health.AddHealth(health);
    }

    private void AddCoin(int coin)
    {
        Coin += coin;
    }

}
