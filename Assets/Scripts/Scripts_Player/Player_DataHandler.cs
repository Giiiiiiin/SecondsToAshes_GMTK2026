using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player_DataHandler : CombatBase
{
    [SerializeField] private PlayerAnimations playerAnimation;
    [SerializeField] private GunWifeAnimations gunWifeAnimation;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _dodgeSpeedMod = 8f;
    [SerializeField] private float _sprintSpeedMod = 8f;
    [SerializeField] private float _attackSpeed = 5f; 

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

    public float attackBuffer = 0f;
    private float baseCooldown;
    private float cooldownShoot = 0f;
    private float cooldownDodge = 0f;
    private float cooldownSwitch = 0f;

    //========================
    //      Get_Set_Status
    //========================

    public bool isMoving
    {
        set { _isMoving = value; }
        get { return _isMoving; }
    }

    public bool isDashing
    {
        set { _isDashing = value; }
        get { return _isDashing; }
    }

    public bool isDodging
    {
        set { _isDodging = value; }
        get { return _isDodging; }
    }

    public bool isMelee
    {
        set { _isMelee = value; }
        get { return _isMelee; }
    }

    public bool isShooting
    {
        set { _isShooting = value; }
        get { return _isShooting; }
    }
    
    public bool isSprinting
    {
        set { _isSprinting = value; }
        get { return _isSprinting; }
    }

    //========================
    //      Get_Set_CanDo
    //========================

    public bool canMove
    {
        set { _canMove = value; }
        get { return _canMove; }
    }

    public bool canDash
    {
        set { _canDash = value; }
        get { return _canDash; }
    }

    public bool canDodge
    {
        set { _canDodge = value; }
        get { return _canDodge; }
    }
    
    public bool canSprint
    {
        set { _canSprint = value; }
        get { return _canSprint; }
    }

    public bool canMelee
    {
        set { _canMelee = value; }
        get { return _canMelee; }
    }

    public bool canShoot
    {
        set { _canShoot = value; }
        get { return _canShoot; }
    }

    private void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();

        if (rb == null) rb = new Rigidbody2D();
        if (gameInput == null) gameInput = new InputSystem_Action();

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

    protected override void Update()
    {
        base.Update();

        baseCooldown = Mathf.Max(0.1f, 1f / Mathf.Max(combatData.attackSpeed, 0.1f));

        if (attackBuffer > 0f)
        {
            attackBuffer -= Time.deltaTime;
            if (attackBuffer <= 0f)
                attackBuffer = 0f;
        }  

        if (cooldownShoot > 0f)
        {
            cooldownShoot -= Time.deltaTime;

            if (cooldownShoot <= 0f)
            {
                cooldownShoot = 0f;
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

        if (attackBuffer == 0f)
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
            Attack();
    }

    private void TryShoot()
    {
        if (!canShoot || isShooting|| cooldownShoot > 0f || cooldownSwitch > 0f)
        {
            Debug.Log("Cannot Shoot Instance!");

            return;
        }

        Debug.Log("Shot!");
        _isShooting = true;

        cooldownShoot += baseCooldown;
        cooldownSwitch = baseCooldown / 2;

        gunWifeAnimation.Shoot(cooldownShoot);
    }

    public override void Attack()
    {
        base.Attack();

        if (!canMelee || isMelee || cooldownSwitch > 0f)
        {
            Debug.Log("Cannot Melee Instance!");

            return;
        }
        Debug.Log("Swing!");

        _isMelee = true;
        cooldownSwitch = baseCooldown / 2;
    }
}
