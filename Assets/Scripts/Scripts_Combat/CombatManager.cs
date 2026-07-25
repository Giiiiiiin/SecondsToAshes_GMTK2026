using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private static CombatManager _instance;
    public static CombatManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    private void CalculateDamage(CombatBase sourceUnit, CombatBase targetUnit)
    {
        float damage = sourceUnit.UnitData.attackDamage - targetUnit.UnitData.hitPoints;
        damage = Mathf.Max(damage, 0);
        targetUnit.UnitData.hitPoints -= damage;
    }

    public void DealDamage(CombatBase sourceUnit, CombatBase targetUnit)
    {
        CalculateDamage(sourceUnit, targetUnit);

        if (targetUnit.UnitData.hitPoints <= 0)
        {
            Destroy(targetUnit.gameObject);
        }

        Debug.Log($"{sourceUnit.name} dealt {sourceUnit.UnitData.attackDamage} damage to {targetUnit.name}. Remaining HP: {targetUnit.UnitData.hitPoints}");
    }
}
