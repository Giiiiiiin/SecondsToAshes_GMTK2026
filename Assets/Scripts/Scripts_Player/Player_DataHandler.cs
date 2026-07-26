using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class Player_DataHandler : CombatBase
{
    [SerializeField] private GunWifeAnimations gunWifeAnimation;

    private Vector2 savedDir = Vector2.zero; // last faced direction
    private bool sprintButtonDown = false; //check if sprint button is held down

    [SerializeField] private float _dodgeSpeedMod = 8f;
    [SerializeField] private float _sprintSpeedMod = 8f;


    private InputSystem_Action gameInput;
    private Vector2 moveInput;

    private Rigidbody2D _rb;

    private bool _isMoving;
    private bool _isDodging = false;
    private bool _isSprinting = false;

    private bool _canMove = false;
    private bool _canDodge = false;
    private bool _canSprint = false;

    // COMBAT THINGS
    private bool _isLunging;

    // combo parameters
    public int _comboIndex = 0;
    private float comboTimer = 0f;

    // actions checks
    public bool _canMelee = false;
    private bool _canShoot = false;

    public bool _isMelee = false;
    private bool _isShooting = false;

    public float attackBuffer = 0f;
    private float baseCooldown;
    private float cooldownShoot = 0f;
    private float cooldownSwitch = 0f;

    [SerializeField] private float _attackSpeed = 5f;

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

    // get rigidbody

    public Rigidbody2D rb
    {
        get { return _rb; }
    }

    // FOR COMBAT
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

    public int comboIndex
    {
        set { _comboIndex = value; }
        get { return _comboIndex; }
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

        //if (cooldownDodge > 0f)
        //{
        //    cooldownDodge -= Time.deltaTime;

        //    if (cooldownDodge <= 0f)
        //        cooldownDodge = 0f;
        //}

        //LungeOnAttack();

        InputSprint();
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

    // COMBAT THINGIES

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
        if (!_canShoot || _isShooting || cooldownShoot > 0f || cooldownSwitch > 0f)
        {
            Debug.Log("Cannot Shoot Instance!");

            return;
        }

        Debug.Log("Shot!");
        _isShooting = true;

        cooldownShoot += baseCooldown;
        cooldownSwitch = baseCooldown / 2;

        // gunWifeAnimation.Shoot(cooldownShoot);
    }

    public override void Attack()
    {
        base.Attack();

        if (!_canMelee || _isMelee)
        {
            Debug.Log("Cannot Melee Instance!");

            return;
        }
        Debug.Log("Swing!");

        //something about the combo timer here

        _canMelee = false;

        _comboIndex++;

        if (_comboIndex > 3)
            _comboIndex = 1;

        _isLunging = true;
        _isMelee = true;

        StartCoroutine(Melee());

        cooldownSwitch = baseCooldown / 2;
    }

    //private void LungeOnAttack()
    //{
    //    Vector2 dir = //get direction of mouse 

    //    // lunge override 
    //    if (_isLunging)
    //    {
    //        _isLunging = false;

    //        float lungeForce = 5;

    //        _rb.linearVelocity = dir * lungeForce;
    //        return;
    //    }
    //}

    private IEnumerator Melee() // controls melee attack timing
    {
        _canMove = false;
        _canDodge = false;
        _isMelee = true;

        yield return null;
    }

    public void endMelee()
    {
        _canMove = true;
        _canDodge = true;
        _canMelee = true;
        _isMelee = false;
    }

}
