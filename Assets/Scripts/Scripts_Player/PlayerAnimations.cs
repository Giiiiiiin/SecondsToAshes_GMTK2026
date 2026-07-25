using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Player_DataHandler playerData;
    [SerializeField] private GameObject playerObject; //Aran

    private AnimationHandler animationHandler;

    private GameObject instanceCombo1_E;
    private GameObject instanceCombo1_W;

    private GameObject instanceCombo2_E;
    private GameObject instanceCombo2_W;

    Vector2 mouseScreenPos;
    Vector3 mousePosition;
    Vector2 mouseDir;

    void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
    }

    private void Start()
    {
        animationHandler.Play(PlayerAnimMovement.PlayerIdle_E);
    }

    void Update()
    {
        mouseScreenPos = Mouse.current.position.value;
        mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        mouseDir = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y);
        
        if (playerData.isMelee)
        {
            if (mouseDir.x > 0)
                animationHandler.Play(PlayerAnimMovement.PlayerMelee_E);
            else
                animationHandler.Play(PlayerAnimMovement.PlayerMelee_W);
        
            return;
        }
        
        if (playerData.isMoving)
        {
            if (mouseDir.x > 0)
                animationHandler.Play(PlayerAnimMovement.PlayerMove_E);
            else
                animationHandler.Play(PlayerAnimMovement.PlayerMove_W);
        
            return;
        }
        
        if (playerData.isDodging || playerData.isSprinting)
        {
            if (mouseDir.x > 0)
                animationHandler.Play(PlayerAnimMovement.PlayerSprint_E);
            else
                animationHandler.Play(PlayerAnimMovement.PlayerSprint_W);
        
            return;
        }
        
        if (mouseDir.x > 0)
            animationHandler.Play(PlayerAnimMovement.PlayerIdle_E);
        else
            animationHandler.Play(PlayerAnimMovement.PlayerIdle_W);
    }

    public void begin_MeleeCombo_1()
    {
        if (instanceCombo1_E != null || instanceCombo1_W != null)
        {
            end_MeleeCombo_1();

            return;
        }

        if (playerData.equippedWeapon != null && instanceCombo1_E == null && instanceCombo1_W == null)
        {
            if (mouseDir.x > 0)
            {
                GameObject gameObject1 = Instantiate(playerData.equippedWeapon.attackList_E[0], playerObject.transform.position + playerData.equippedWeapon.offsetAttackList_E[0], Quaternion.identity, playerObject.transform);
                instanceCombo1_E = gameObject1;

                AttackObject attackObject = gameObject1.GetComponent<AttackObject>();
                Debug.Log("AttackObject: " + attackObject);
                if (attackObject != null)
                {
                    attackObject.sourceUnit = playerData;
                    Debug.Log("AttackObject sourceUnit: " + attackObject.sourceUnit);
                }
            }
            else
            {
                GameObject gameObject1 = Instantiate(playerData.equippedWeapon.attackList_W[0], playerObject.transform.position + playerData.equippedWeapon.offsetAttackList_W[0], Quaternion.identity, playerObject.transform);
                instanceCombo1_W = gameObject1;

                AttackObject attackObject = gameObject1.GetComponent<AttackObject>();
                if (attackObject != null)
                    attackObject.sourceUnit = playerData;
            }
        }
    }

    public void end_MeleeCombo_1()
    {
        if (instanceCombo1_E)
        {
            Destroy(instanceCombo1_E);
        }

        if (instanceCombo1_W)
        {
            Destroy(instanceCombo1_W);
        }   

        playerData.isMelee = false;
    }
}
