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

    [SerializeField] private string idleAnimation_E;
    [SerializeField] private string idleAnimation_W;

    [SerializeField] private string walkAnimation_E;
    [SerializeField] private string walkAnimation_W;

    [SerializeField] private string initialCombatAnimation_E;
    [SerializeField] private string initialCombatAnimation_W;

    [SerializeField] private float circleAggroRadius = 3f;
    [SerializeField] private float circleAttackRadius = 1f;
    private bool isAttacking = false;

    private Vector2 targetDir;
    private float attackBuffer = 0f;
    
    public AnimationHandler AnimationHandler => animationHandler;
    public Vector2 TargetDir => targetDir;
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

        if (!isAttacking)
        {
            if (targetDir.x > 0)
            {
                if (idleAnimation_E != null && idleAnimation_E != "")
                    animationHandler.Play(idleAnimation_E);
            }
            else
            {
                if (idleAnimation_W != null && idleAnimation_W != "")
                    animationHandler.Play(idleAnimation_W);
            }
        }

        if (attackBuffer > 0f)
        {
            attackBuffer -= Time.deltaTime;

            if (attackBuffer <= 0f)
                attackBuffer = 0f;
        }

        if (distance >= circleAggroRadius)
        {
            rb.linearVelocity = Vector2.zero;

            isAttacking = false;
        }
        else
        {
            rb.linearVelocity = targetDir.normalized * speed;

            if (targetDir.x > 0)
            {
                if (walkAnimation_E != null && walkAnimation_E != "")
                    animationHandler.Play(walkAnimation_E);
            }
            else
            {
                if (walkAnimation_W != null && walkAnimation_W != "")
                    animationHandler.Play(walkAnimation_W);
            }
        }

        if (distance <= circleAttackRadius)
        {
            rb.linearVelocity = Vector2.zero;

            Attack();
        }
    }

    public override void Attack()
    {
        Debug.Log("BaseEnemyLogic Attack: " + gameObject.name + " | AttackCooldown: " + attackCooldown);
        base.Attack();
        isAttacking = true;

        if (isAttacking)
        {
            if (targetDir.x > 0)
            {
                if (initialCombatAnimation_E != null && initialCombatAnimation_E != "")
                    animationHandler.Play(initialCombatAnimation_E);
            }
            else
            {
                if (initialCombatAnimation_W != null && initialCombatAnimation_W != "")
                    animationHandler.Play(initialCombatAnimation_W);
            }
        }

        return;
    }
}
