using UnityEngine;

public class AttackObject : MonoBehaviour
{
    public CombatBase sourceUnit;
    public FixedJoint2D _fixedJoint;

    private Collider2D _collider;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        Debug.Log("AttackObject SourceUnit Awake: " + sourceUnit.gameObject);

        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _fixedJoint = GetComponent<FixedJoint2D>();
    }

    private void Start()
    {
        Debug.Log("AttackObject SourceUnit Start: " + sourceUnit.gameObject);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("AttackObject SourceUnit: " + sourceUnit.name);
        Debug.Log("AttackObject CollisionUnit: " + collision.gameObject);

        CombatBase unit = collision.gameObject.GetComponent<CombatBase>();

        if (unit != null && unit != sourceUnit)
        {
            sourceUnit.OnHit();
            unit.OnDamage(sourceUnit, unit);
        }
    }
}
