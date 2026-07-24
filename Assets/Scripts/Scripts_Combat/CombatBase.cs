using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBase : MonoBehaviour
{
    [SerializeField] private CombatUnit_ScriptableObject combatData;
    [SerializeField] private List<GameObject> inventory;

    public GameObject Source => gameObject;
    [SerializeField] private CombatWeapon_ScriptableObject equippedWeapon;

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
}
