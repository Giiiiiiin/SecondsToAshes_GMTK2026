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
        else
        {
            Vector2 velocity = Vector2.zero;
            velocity = targetDir * combatData.unitSpeed;

            rb.linearVelocity = targetDir.normalized * speed;
            rb.AddForce(velocity * Time.deltaTime, ForceMode2D.Impulse);

            if (rb.linearVelocity != Vector2.zero)
                savedDir = rb.linearVelocity; 

            if (walkAnimation != null && walkAnimation != "")
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
        float addCooldown = 10f;
        attackCooldown += addCooldown;

        Debug.Log("Add Cooldown called: " + attackCooldown);
    }

    public override void Attack()
    {
        if (attackCooldown > 0f)
        {
            Debug.Log("Attack is on Cooldown");
            return;
        }

        Debug.Log("BaseEnemyLogic Attack: " + gameObject.name + " | AttackCooldown: " + attackCooldown);

        isAttacking = true;

        if (initialCombatAnimation != null && initialCombatAnimation != "")
        {
            Debug.Log("Animation Called");
            animationHandler.Play(initialCombatAnimation);
        }

        return;
    }
}
