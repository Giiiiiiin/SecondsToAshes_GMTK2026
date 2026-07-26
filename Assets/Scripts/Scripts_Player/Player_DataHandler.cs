using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player_DataHandler : CombatBase
{
    [SerializeField] private PlayerAnimations playerAnimation;
    // [SerializeField] private GunWifeAnimations gunWifeAnimation;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _dodgeSpeedMod = 8f;
    [SerializeField] private float _sprintSpeedMod = 8f;
    [SerializeField] private float _attackSpeed = 5f;

    public static UnityEvent ShootGun = new UnityEvent();

    private InputSystem_Action gameInput;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    private bool _isMoving = false;
    private bool _isDashing = false;
    private bool _isDodging = false;
    private bool _isSprinting = false;
    private bool _isMelee = false;
    private bool _isShooting = false;

    private bool _canMove = false;
    private bool _canDash = false;
    private bool _canDodge = false;
    private bool _canSprint = false;
    private bool _canMelee = false;
    private bool _canShoot = false;

    private float baseCooldown;
    private float cooldownShoot = 0f;
    private float cooldownMelee = 0f;
    private float cooldownDodge = 0f;
    private float cooldownSwitch = 0f;

    //========================
    //      GET_IsStatus
    //========================

    public bool isMoving
    {
        get { return _isMoving; }
    }

    public bool isDashing
    {
        get { return _isDashing; }
    }

    public bool isDodging
    {
        get { return _isDodging; }
    }

    public bool isMelee
    {
        get { return _isMelee; }
    }

    public bool isShooting
    {
        get { return _isShooting; }
    }
    
    public bool isSprinting
    {
        get { return _isSprinting; }
    }

    //========================
    //      GET_CanStatus
    //========================

    public bool canMove
    {
        get { return _canMove; }
    }

    public bool canDash
    {
        get { return _canDash; }
    }

    public bool canDodge
    {
        get { return _canDodge; }
    }
    
    public bool canSprint
    {
        get { return _canSprint; }
    }

    public bool canMelee
    {
        get { return _canMelee; }
    }

    public bool canShoot
    {
        get { return _canShoot; }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null) rb = new Rigidbody2D();
        if (gameInput == null) gameInput = new InputSystem_Action();

        baseCooldown = Mathf.Max(0.1f, 1f / Mathf.Max(combatData.attackSpeed, 0.1f));
        _canMove = true;
        _canDash = true;
        _canDodge = true;
        _canSprint = true;
        _canMelee = true;
        _canShoot = true;
    }

private void OnEnable()
    {
        gameInput.Enable();
    }

    private void OnDisable()
    {
        gameInput.Disable();
    }

    private void Update()
    {
        if (cooldownMelee > 0f)
        {
            cooldownMelee -= Time.deltaTime;

            if (cooldownMelee <= 0f)
            {
                cooldownMelee = 0f;
                _isMelee = false;
            }
        }

        if (cooldownShoot > 0f)
        {
            cooldownShoot -= Time.deltaTime;

            if (cooldownShoot <= 0f)
            {
                cooldownShoot = 0f;
                _canShoot = true;
                _isShooting = false;
            }
        }

        if (cooldownDodge > 0f)
        {
            cooldownDodge -= Time.deltaTime;

            if (cooldownDodge <= 0f)
                cooldownDodge = 0f;
        }

        if (cooldownSwitch > 0f)
        {
            cooldownSwitch -= Time.deltaTime;

            if (cooldownSwitch <= 0f)
                cooldownSwitch = 0f;
        }
    }

    private void FixedUpdate()
    {
        InputMovement();
        InputAttack();
    }

    private void InputMovement()
    {
        moveInput = gameInput.Player.Move.ReadValue<Vector2>();
        var dodgeInput = gameInput.Player.Dodge.ReadValue<float>();
        var sprintInput = gameInput.Player.Sprint.ReadValue<float>();
        
        Vector2 movement = moveInput.normalized;
        Vector2 velocity = Vector2.zero;

        if (movement.magnitude > 1f)
            movement.Normalize();

        if (canMove && movement != Vector2.zero && combatData.unitSpeed != 0f)
        {
            _isMoving = true;

            velocity = movement * combatData.unitSpeed;
            rb.linearVelocity = velocity; 
        }
        else
        {
            velocity = Vector2.zero;
            _isMoving = false;
        }

        if (canDodge && dodgeInput > 0f)
        {
            _isDodging = true;
            velocity = movement * (combatData.unitSpeed + _dodgeSpeedMod);
        }
        else if (canSprint && sprintInput > 0f)
        {
            _canSprint = true;
            velocity = movement * (combatData.unitSpeed + _sprintSpeedMod);
        } 
        
        rb.linearVelocity = velocity;
}

    private void InputAttack()
    {
        float shoot = gameInput.Player.Shoot.ReadValue<float>();
        float melee = gameInput.Player.Melee.ReadValue<float>();

        if (shoot > 0f)
            TryShoot();

        if (melee > 0f)
            TryMelee();
    }

    private void TryShoot()
    {
        if (!canShoot || isShooting || cooldownShoot > 0f || cooldownSwitch > 0f)
        {
            Debug.Log("Cannot Shoot Instance!");
            return;
        }

        Debug.Log("Shot!");
        ShootGun.Invoke();
        
        _isShooting = true;
        cooldownShoot += baseCooldown;
        cooldownSwitch = baseCooldown / 2;
    }

    public void EndShoot()
    {
        _isShooting = false;
        _canShoot = false;
    }

    private void TryMelee()
    {
        if (!canMelee || isMelee || cooldownMelee > 0f || cooldownSwitch > 0f)
        {
            Debug.Log("Cannot Melee Instance!");

            return;
        }
        
        Debug.Log("Swung!");
        _isMelee = true;

        cooldownMelee += baseCooldown;
        cooldownSwitch = baseCooldown / 2;

        playerAnimation.Melee(cooldownMelee);
    }
    
    public void EndMelee()
    {
        _isMelee = false;
        _canMelee = true;
    }
}
