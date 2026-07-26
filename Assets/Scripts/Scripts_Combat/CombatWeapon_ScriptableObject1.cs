using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponUnit", menuName = "Combat_SO/WeaponUnit")]
public class CombatWeapon_ScriptableObject : ScriptableObject
{
    public Transform Source;
    public List<GameObject> attackList = new List<GameObject>();

    public string UnitName;
    public string UnitSerialNumber;

    //these valuew vary depending on the user's base stats
    public float attackDamage;
    public float attackSpeed;
    public float armorDefense;
    public float armorPenetration;
    public float hitPoints;
    public float unitSpeed;
}
