using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    public bool destroyOnHit = false;
    public CombatBase sourceUnit;
    public FixedJoint2D _fixedJoint;

    private Collider2D _collider;
    private Rigidbody2D _rigidbody;

    private List<CombatBase> attackedUnits;

    private void Awake()
    {
        attackedUnits = new List<CombatBase>();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _fixedJoint = GetComponent<FixedJoint2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("AttackObject SourceUnit: " + sourceUnit.name);
        Debug.Log("AttackObject CollisionUnit: " + collision.gameObject);

        CombatBase targetUnit = collision.gameObject.GetComponent<CombatBase>();

        if (targetUnit == null) return;
        
        if (attackedUnits.Contains(targetUnit))
            Debug.Log($"{targetUnit.name} has already been attacked");
        else if (targetUnit != sourceUnit)
        {
            CombatManager.Instance.DealDamage(sourceUnit, targetUnit);
            attackedUnits.Add(targetUnit);
        }
    }
}
