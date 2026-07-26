using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private BaseEnemyLogic enemyLogic;
    public bool attachJoint = true;

    private int attackDirection;
    private GameObject instanceCombo1;

    [SerializeField] private List<string> comboAnimations = new List<string>();

    private void FaceDirection(Rigidbody2D rb)
    {
        Vector3 localScale = transform.localScale;

        if (enemyLogic.SavedDir.x > 0f)
        {
            localScale.x = Mathf.Abs(localScale.x); // right
            attackDirection = 1;

        }
        else if (enemyLogic.SavedDir.x < 0f)
        {
            localScale.x = -Mathf.Abs(localScale.x); // left
            attackDirection = -1;

        }
        base.transform.localScale = localScale;
    }


    public void begin_Combo_1()
    {
        Debug.Log("begin_Combo_1 called");
        if (instanceCombo1 != null)
        {
            Debug.Log("Combo already in progress. Ending current combo.");

            return;
        }

        GameObject gameObject1 = Instantiate(enemyLogic.equippedWeapon.attackList[0], enemyLogic.transform.position + (enemyLogic.equippedWeapon.offsetAttackList[0] * attackDirection), Quaternion.identity, enemyLogic.transform);
        instanceCombo1 = gameObject1;

        FaceDirection(gameObject1.GetComponent<Rigidbody2D>());

        AttackObject attackObject = gameObject1.GetComponent<AttackObject>();
        if (attackObject != null)
        {
            attackObject.sourceUnit = enemyLogic;
            if (attachJoint)
                attackObject._fixedJoint.connectedBody = enemyLogic.GetComponent<Rigidbody2D>();  
        }
    }

    public void end_Combo_1()
    {
        Debug.Log("end_Combo_1 called");

        if (instanceCombo1 != null)
        {
            Destroy(instanceCombo1.gameObject);
            instanceCombo1 = null;
            Debug.Log("end_Combo_1 called and instance destroyed");
        }

        enemyLogic.IsAttacking = false;
    }
}
