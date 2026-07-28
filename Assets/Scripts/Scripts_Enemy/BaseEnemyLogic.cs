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

    [SerializeField] private float circleAggroRadius = 3f;
    [SerializeField] private float circleAttackRadius = 1f;
    private bool isAttacking = false;

    private Vector2 savedDir = Vector2.zero;
    private Vector2 targetDir;
    private float attackBuffer = 0f;
    
    public AnimationHandler AnimationHandler => animationHandler;
    public Vector2 TargetDir => targetDir;
    public Vector2 SavedDir => savedDir;

    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
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

        if (distance > circleAggroRadius)
        {
            rb.linearVelocity = Vector2.zero;

            isAttacking = false;

            if (idleAnimation != null && idleAnimation != "")
                animationHandler.Play(idleAnimation);
        }
        else if (distance < circleAttackRadius)  //If enemy is already within attack range, stop attacking.
        {
            Vector2 velocity = Vector2.zero;
            velocity = targetDir * combatData.unitSpeed;

            rb.linearVelocity = targetDir.normalized * speed;
            rb.AddForce(velocity * Time.deltaTime, ForceMode2D.Impulse);

            if (rb.linearVelocity != Vector2.zero)
                savedDir = rb.linearVelocity; 

            if (walkAnimation != null && walkAnimation != "" && !isAttacking)
                animationHandler.Play(walkAnimation);
        }

        FaceDirection();

        if (distance <= circleAttackRadius)
        {
            rb.linearVelocity = Vector2.zero;

            Attack();
        }
    }

    private void FaceDirection()
    {
        Vector3 localScale = transform.localScale;

        if (rb.linearVelocity.x > 0f)
        {
            localScale.x = Mathf.Abs(localScale.x); // right
        }
        else if (rb.linearVelocity.x < 0f)
        {
            localScale.x = -Mathf.Abs(localScale.x); // left
        }
        base.transform.localScale = localScale;
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

        isAttacking = true;
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
        if (source.GetComponent<PlayerMarker>() != null) return;
        
        currentHitPoints = targetHitPoints;
        OnDamageEffect.Invoke();
        
        if (currentHitPoints <= 0)
            OnDeath();
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance == null)
            Debug.LogError("CombatManager not found!");
            
        CombatManager.Instance.OnEnemyDeath();
        Destroy(gameObject);
    }
}
