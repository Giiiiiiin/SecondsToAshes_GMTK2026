using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CombatBase : MonoBehaviour
{
    [SerializeField] protected CombatUnit_ScriptableObject combatData;
    [SerializeField] protected List<GameObject> inventory;

    public GameObject Source => gameObject;
    public CombatWeapon_ScriptableObject equippedWeapon;

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

    private void Awake()
    {
        combatData = Instantiate(combatData);
        combatData.Source = gameObject;

        if (combatData.Weapon != null)
            Weapon = combatData.Weapon;
    }

    private void Start()
    {
        Debug.Log("Unit DATA: " + UnitData.Source);
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

    public void EquipWeapon (string weaponName)
    {
        Weapon = SearchInventoryWeapon(weaponName);
    }

    public virtual void Attack()
    {
        // Base function that is called by all Units to Attack
    }

    public virtual void OnHit()
    {
        // Base function that is called by all Units if thir Attacks hit a valid target
    }

    public virtual void OnDamage()
    {
        // Base function that is called by all Units if they are Damaged by an Attack
    }
}
