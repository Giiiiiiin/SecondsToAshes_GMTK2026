using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private BaseEnemyLogic enemyLogic;

    private GameObject instanceCombo1_E;
    private GameObject instanceCombo1_W;

    [SerializeField] private List<string> comboAnimations_E = new List<string>();
    [SerializeField] private List<string> comboAnimations_W = new List<string>();
    public void begin_Combo_1()
    {
        Debug.Log("begin_Combo_1 called");
        if (instanceCombo1_E != null || instanceCombo1_W != null)
        {
            Debug.Log("Combo already in progress. Ending current combo.");
            end_Combo_1();

            return;
        }

        if (enemyLogic.TargetDir.x > 0)
        {
            GameObject gameObject1 = Instantiate(enemyLogic.equippedWeapon.attackList_E[0], enemyLogic.transform.position + enemyLogic.equippedWeapon.offsetAttackList_E[0], Quaternion.identity, enemyLogic.transform);
            instanceCombo1_E = gameObject1;

            AttackObject attackObject = gameObject1.GetComponent<AttackObject>();
            Debug.Log("AttackObject: " + attackObject);
            if (attackObject != null)
            {
                attackObject.sourceUnit = enemyLogic;
                Debug.Log("AttackObject sourceUnit: " + attackObject.sourceUnit);
            }

            enemyLogic.AttackBuffer += enemyLogic.equippedWeapon.attackBufferList[0];
        }
        else
        {
            GameObject gameObject1 = Instantiate(enemyLogic.equippedWeapon.attackList_W[0], enemyLogic.transform.position + enemyLogic.equippedWeapon.offsetAttackList_W[0], Quaternion.identity, enemyLogic.transform);
            instanceCombo1_W = gameObject1;

            AttackObject attackObject = gameObject1.GetComponent<AttackObject>();
            if (attackObject != null)
                attackObject.sourceUnit = enemyLogic;
        }

        enemyLogic.AddCooldown();
    }

    public void end_Combo_1()
    {
        if (instanceCombo1_E)
        {
            Destroy(instanceCombo1_E);
        }

        if (instanceCombo1_W)
        {
            Destroy(instanceCombo1_W);
        }

        enemyLogic.IsAttacking = false;
    }
}
