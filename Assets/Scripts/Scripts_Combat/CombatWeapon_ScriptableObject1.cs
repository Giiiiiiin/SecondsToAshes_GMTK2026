using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponUnit", menuName = "Combat_SO/WeaponUnit")]
public class CombatWeapon_ScriptableObject : ScriptableObject
{
    public Sprite Source;
    public List<float> attackBufferList = new List<float>();
    public List<GameObject> attackList = new List<GameObject>();
    public List<Vector3> offsetAttackList = new List<Vector3>();

    public string UnitName;
    public string UnitSerialNumber;

    //these valuew vary depending on the user's base stats
    public float attackDamage;
    public float attackSpeed;
    public float hitPoints;
    public float unitSpeed;
}
