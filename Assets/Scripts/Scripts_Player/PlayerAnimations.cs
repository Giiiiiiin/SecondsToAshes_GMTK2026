
using UnityEngine;


public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Player_DataHandler playerData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private AnimationHandler animationHandler;
    private GameObject instanceCombo;
    private float bufferCounter;
    private int attackDirection;

    void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
    }

    void Update()
    {
        if (playerData.comboIndex >= 1 && instanceCombo != null)
        {
            bufferCounter -= Time.deltaTime;
            
            if (bufferCounter <= 0f)
                playerData.comboIndex = 1;
        }
            
            
        HandleAnimation();
        FaceDirection(base.gameObject);
    }

    private void HandleAnimation()
    {
        int combo = playerData.comboIndex;
        string comboState;

        //melee attack
        if (playerData.isMelee)
        {
            comboState = "PlayerMelee" + combo;
            
            animationHandler.Play(comboState);
            return;
        }

        //dodge
        if (playerData.isDodging)
        {
            animationHandler.Play(PlayerAnimMovement.PlayerDodge);
            return;
        }

        //sprint
        if (playerData.isMoving && playerData.isSprinting)
        {
            animationHandler.Play(PlayerAnimMovement.PlayerSprint);
            return;
        }

        //walk
        if (playerData.isMoving)
        {
            animationHandler.Play(PlayerAnimMovement.PlayerWalk);
            return;
        }

        //idle (fallback)
        animationHandler.Play(PlayerAnimMovement.PlayerIdle);
    }

    private void FaceDirection(GameObject g)
    {
        Vector3 localScale = g.transform.localScale;

        if (playerData.rb.linearVelocity.x > 0f)
        {
            localScale.x = Mathf.Abs(localScale.x); // right
            attackDirection = 1;
        }
        else if (playerData.rb.linearVelocity.x < 0f)
        {
            localScale.x = -Mathf.Abs(localScale.x); // left
            attackDirection = -1;
        }
        g.transform.localScale = localScale;
    }

    public void EndDodge() 
    {
        playerData.EndDodge();
    }

    public void EndMelee()
    {
        EndCombo();
        playerData.EndMelee();
      
    }
    
    public void Begin_Combo()
    {
        Debug.Log("Begin_Combo");
        if (instanceCombo != null)
        {
            Debug.Log("Combo already in progress. Ending current combo.");
            return;
        }

        //Move these to playuerdatahandler
        GameObject c = Instantiate(playerData.equippedWeapon.attackList[playerData.comboIndex-1], playerData.transform.position + (playerData.equippedWeapon.offsetAttackList[playerData.comboIndex-1] * attackDirection), Quaternion.identity, playerData.transform);
        instanceCombo = c;
        
        FaceDirection(c);

        AttackObject attackObject = c.GetComponent<AttackObject>();
        if (attackObject != null)
        {
            attackObject.sourceUnit = playerData;
            attackObject._fixedJoint.connectedBody = playerData.GetComponent<Rigidbody2D>();  
        }
        
        Debug.Log("Instantiated Attack Object");
    }

    private void EndCombo()
    {
        Debug.Log("EndCombo");
        
        //move these to playerdatahandler
        if (instanceCombo != null)
        {
            instanceCombo.gameObject.SetActive(false);
            Destroy(instanceCombo.gameObject);
            
            Debug.Log("Destroyed Attack");
            bufferCounter = playerData.equippedWeapon.attackBufferList[playerData.comboIndex-1];
        }
        playerData.isMelee = false;
    }
    
    private void DamageEffect()
    {
        StartCoroutine(playerData.SpriteHitEffect(spriteRenderer));
    }
    
    private void OnEnable()
    {
        playerData.OnDamageEffect.AddListener(DamageEffect);
    }

    private void OnDisable()
    {
        playerData.OnDamageEffect.RemoveListener(DamageEffect);
    }
}
