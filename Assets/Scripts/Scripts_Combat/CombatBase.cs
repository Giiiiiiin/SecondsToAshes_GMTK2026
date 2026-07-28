using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CombatBase : MonoBehaviour
{
    [SerializeField] protected CombatUnit_ScriptableObject combatData;
    [SerializeField] protected List<GameObject> inventory;

    public float currentHitPoints;
    public GameObject Source => gameObject;
    public CombatWeapon_ScriptableObject equippedWeapon;
    protected float attackCooldown = 0f;
    
    public UnityEvent OnDamageEffect = new UnityEvent();
    
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

    public void EquipWeapon (string weaponName)
    {
        Weapon = SearchInventoryWeapon(weaponName);
    }

    protected abstract void Attack();
    public abstract void OnReceiveDamage(CombatBase source, float damage);
    protected abstract void OnDeath();
    
    //unsure if I want this here, because its a vfx thing, probably should just be its own seperate thing
    public virtual IEnumerator SpriteHitEffect(SpriteRenderer sr)
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
    }
}
