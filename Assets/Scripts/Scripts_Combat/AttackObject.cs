using UnityEngine;

public class AttackObject : MonoBehaviour
{
    public CombatBase sourceUnit;

    private Collider2D _collider;

    private void Awake()
    {
        Debug.Log("AttackObject SourceUnit Awake: " + sourceUnit);

        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Debug.Log("AttackObject SourceUnit Start: " + sourceUnit);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("AttackObject SourceUnit: " + sourceUnit);
        Debug.Log("AttackObject CollisionUnit: " + collision.gameObject);

        CombatBase unit = collision.gameObject.GetComponent<CombatBase>();

        if (unit != null && unit != sourceUnit)
        {
            sourceUnit.OnHit();
            unit.OnDamage(sourceUnit, unit);
        }
    }
}
