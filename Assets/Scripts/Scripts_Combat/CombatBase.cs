using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CombatBase : MonoBehaviour
{
    [SerializeField] protected CombatUnit_ScriptableObject combatData;
    [SerializeField] protected List<GameObject> inventory;

    public float currentHitPoints;
    public GameObject Source => gameObject;
    public CombatWeapon_ScriptableObject equippedWeapon;
    protected float attackCooldown = 0f;
    public CombatUnit_ScriptableObject UnitData
    {
        get { return combatData; }
    }

    public List<GameObject> Inventory
    {
        get { return inventory; }
    }

    public CombatWeapon_ScriptableObject Weapon
    {
        get => equippedWeapon;
        set => equippedWeapon = value;
    }

    protected virtual void Awake()
    {
        combatData = Instantiate(combatData);
        combatData.Source = gameObject;

        if (combatData.Weapon != null)
            Weapon = combatData.Weapon;
    }

    private void Start()
    {
        currentHitPoints = combatData.hitPoints;

        Debug.Log("Unit DATA: " + UnitData.Source);
        Debug.Log("Unit WEAPON: " + equippedWeapon);
    }

    public CombatWeapon_ScriptableObject SearchInventoryWeapon(string weaponName)
    {
        IList list = inventory;
        for (int i = 0; i < list.Count; i++)
        {
            CombatWeapon_ScriptableObject weapon = (CombatWeapon_ScriptableObject)list[i];
            if (weapon.name == weaponName && weapon is CombatWeapon_ScriptableObject)
            {
                return weapon;
            }
        }

        Debug.Log("Weapon Name is not in Inventory");
        return null;
    }

    protected virtual void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;

            if (attackCooldown <= 0f)
                attackCooldown = 0f;
        }
    }

    public void AddCooldown()
    {
        float addCooldown = Mathf.Max(0.1f, 1f / Mathf.Max(combatData.attackSpeed, 0.1f));
        attackCooldown += addCooldown;
    }

    public void EquipWeapon (string weaponName)
    {
        Weapon = SearchInventoryWeapon(weaponName);
    }

    public virtual void Attack()
    {
        if (attackCooldown > 0f)
        {
            Debug.Log("Attack is on Cooldown");
            return;
        }

        float addCooldown = Mathf.Max(0.1f, 1f / Mathf.Max(combatData.attackSpeed, 0.1f));
        attackCooldown += addCooldown;

        // Base function that is called by all Units to Attack
    }

    public virtual void OnHit()
    {
        // Base function that is called by all Units if thir Attacks hit a valid target
    }

    public virtual void OnDamage(CombatBase source, CombatBase target)
    {
        PlayerMarker playerMarker = target.GetComponent<PlayerMarker>();

        if (CombatManager.Instance != null && playerMarker != true)
            CombatManager.Instance.DealDamage(source, target);

        // Base function that is called by all Units if they are Damaged by an Attack
    }
}
