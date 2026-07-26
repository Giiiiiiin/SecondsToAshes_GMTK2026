using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatUnit", menuName = "Combat_SO/CombatUnit")]
public class CombatUnit_ScriptableObject : ScriptableObject
{
    public GameObject Source;
    public CombatWeapon_ScriptableObject Weapon;

    public string UnitName;
    public string UnitSerialNumber;

    public float attackDamage;
    public float attackSpeed;
    public float armorDefense;
    public float armorPenetration;
    public float hitPoints;
    public float unitSpeed;
}
