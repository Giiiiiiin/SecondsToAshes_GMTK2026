using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseEnemyLogic : CombatBase
{
    private Rigidbody2D rb;
    private PlayerMarker playerMarker;
    private AnimationHandler animationHandler;
    private CombatUnit_ScriptableObject combatData => UnitData;

    [SerializeField] private string idleAnimation;
    [SerializeField] private string walkAnimation;
    [SerializeField] private string initialCombatAnimation;
    [SerializeField] private string deathAnimation;

    [SerializeField] private float circleAggroRadius = 3f;
    [SerializeField] private float circleAttackRadius = 1f;
    private bool _isAttacking = false;
    private bool _isDefeated = false;

    private Vector2 savedDir = Vector2.zero;
    private Vector2 targetDir;
    private float attackBuffer = 0f;
    
    public AnimationHandler AnimationHandler => animationHandler;
    public Vector2 TargetDir => targetDir;
    public Vector2 SavedDir => savedDir;

    public bool IsAttacking
    {
        get { return _isAttacking; }
        set { _isAttacking = value; }
    }
    
    public bool IsDefeated
    {
        get { return _isDefeated; }
        set { _isDefeated = value; }
    }
    public float AttackBuffer
    {
        get { return attackBuffer; }
        set { attackBuffer = value; }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMarker = PlayerMarker.Instance;
        animationHandler = GetComponentInChildren<AnimationHandler>();
    }

    private void Update()
    {
        base.Update();
        if (playerMarker == null)
            playerMarker = PlayerMarker.Instance;

        targetDir = new Vector2(
            playerMarker.transform.position.x - transform.position.x,
            playerMarker.transform.position.y - transform.position.y);

        float speed = combatData.unitSpeed;
        float distance = targetDir.magnitude;

        if (_isDefeated)
        {
            OnDeath();
            return;
        }
        
        if (distance > circleAggroRadius)
        {
            rb.linearVelocity = Vector2.zero;

            _isAttacking = false;

            if (idleAnimation != null && idleAnimation != "")
                animationHandler.Play(idleAnimation);
        }
        else // (distance < circleAttackRadius)  //If enemy is already within attack range, stop attacking.
        {
            Vector2 velocity = Vector2.zero;
            velocity = targetDir * combatData.unitSpeed;

            rb.linearVelocity = targetDir.normalized * speed;
            rb.AddForce(velocity * Time.deltaTime, ForceMode2D.Impulse);

            if (rb.linearVelocity != Vector2.zero)
                savedDir = rb.linearVelocity; 

            if (walkAnimation != null && walkAnimation != "" && !_isAttacking)
                animationHandler.Play(walkAnimation);
        }

        Combat_Utils.FaceDirection(rb, transform);

        if (distance <= circleAttackRadius)
        {
            rb.linearVelocity = Vector2.zero;

            Attack();
        }
    }
    
    public void AddCooldown()
    {
        float addCooldown = Mathf.Max(0f, 1 / combatData.attackSpeed);
        attackCooldown += addCooldown;

        Debug.Log("Add Cooldown called: " + attackCooldown);
    }

    protected override void Attack()
    {
        if (attackCooldown > 0f)
            return;

        _isAttacking = true;
        if (initialCombatAnimation != null && initialCombatAnimation != "")
        {
            animationHandler.Play(initialCombatAnimation); 
            Debug.Log("Animation Called");
        }
        
        AddCooldown();
        Debug.Log("BaseEnemyLogic Attack: " + gameObject.name + " | AttackCooldown: " + attackCooldown);
    }

    public override void OnReceiveDamage(CombatBase source, float targetHitPoints)
    {
        if (source.GetComponent<PlayerMarker>() == null) return;
        
        currentHitPoints -= targetHitPoints;
        OnDamageEffect.Invoke();

        if (currentHitPoints <= 0)
            _isDefeated = true;
    }

    protected override void OnDeath()
    {
        CombatManager.Instance.OnEnemyDeath();
        DisableEnemyMovement();
        animationHandler.Play(deathAnimation);
    }

    private void DisableEnemyMovement()
    {
        _isAttacking = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().isTrigger = true;
    }
    
    public void RemoveFromField()
    {
        gameObject.SetActive(false);
    }
}
