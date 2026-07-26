using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class Player_DataHandler : CombatBase
{
    [SerializeField] private PlayerAnimations playerAnimation;
    // [SerializeField] private GunWifeAnimations gunWifeAnimation;

    [SerializeField] private Player_CombatData playerCombat; // player combat things

    private Vector2 savedDir = Vector2.zero; // last faced direction
    private bool sprintButtonDown = false; //check if sprint button is held down

    [SerializeField] private float _dodgeSpeedMod = 8f;
    [SerializeField] private float _sprintSpeedMod = 8f;
    [SerializeField] private float _attackSpeed = 5f;

    public static UnityEvent ShootGun = new UnityEvent();

    private InputSystem_Action gameInput;
    private Vector2 moveInput;

    private Rigidbody2D _rb;

    private bool _isMoving;
    private bool _isDodging = false;
    private bool _isSprinting = false;

    private bool _isMelee = false;
    private bool _isShooting = false;

    private bool _canMove = false;
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

    // =====

    public Rigidbody2D rb
    {
        get { return _rb; }
    }

    private void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody2D>();

        if (_rb == null) _rb = new Rigidbody2D();
        if (gameInput == null) gameInput = new InputSystem_Action();

        _canMove = true;
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

        InputSprint();

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

        if (movement != Vector2.zero)
        {
            savedDir = movement; // saves last input direction before movement lock
        }

        if (_canMove)
        {
            if (movement != Vector2.zero)
            {
                _isMoving = true;
                velocity = movement * combatData.unitSpeed;

                if (_isSprinting)
                {
                    velocity = movement * (combatData.unitSpeed + _sprintSpeedMod);
                }
            }
            else
            {
                _isMoving = false;
            }
        }

        _rb.AddForce(velocity * Time.deltaTime, ForceMode2D.Impulse);
    }

    private void InputSprint() // also executes dash if able
    {
        if (gameInput.Player.Dodge.IsPressed())
        {
            _isSprinting = true;
            if (canDodge && !sprintButtonDown) {  // only allow the dash once per sprint hold

                canDodge = false;
                _isDodging = true;
                StartCoroutine(Dodge());
            }
            sprintButtonDown = true;
        }
        else
        {
            _isSprinting = false;
            sprintButtonDown = false;
        }
    }

    private IEnumerator Dodge() // coroutine to control dash timing
    {
        _isDodging = true;
        _canMove = false;

        Vector2 dashDirection = moveInput;

        if (dashDirection == Vector2.zero)  // previous direction as fallback
        {
            dashDirection = savedDir;
        }

        if (dashDirection == Vector2.zero)  // initial fallback
        {
            dashDirection = Vector2.right;
        }
        _rb.linearVelocity = dashDirection.normalized * _dodgeSpeedMod; // burst movement from dash

        yield return null;
    }

    public void EndDodge() // end dash based on animation (event in animator)
    {
        _isDodging = false;
        _canDodge = true;
        _canMove = true;
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
    
    public void EndMelee()
    {
        _isMelee = false;
        _canMelee = true;
    }
}
