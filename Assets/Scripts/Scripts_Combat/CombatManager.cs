using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private static CombatManager _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    public void DealDamage(CombatUnit_ScriptableObject sourceUnit, CombatUnit_ScriptableObject targetUnit)
    {
        
    }

    public void SpawnAttack(CombatWeapon_ScriptableObject weaponUnit)
    {

    }
}
