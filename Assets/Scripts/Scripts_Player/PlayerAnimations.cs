
using UnityEngine;


public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Player_DataHandler playerData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private AnimationHandler animationHandler;
    private GameObject instanceCombo;
    private float bufferCounter;

    void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
    }

    void Update()
    {
        if (playerData.comboIndex >= 1)
        {
            bufferCounter -= Time.deltaTime;
            
            if (bufferCounter <= 0f)
                playerData.comboIndex = 1;
        }
            
        HandleAnimation();
        Combat_Utils.FaceDirection(playerData.rb, playerData.transform);
    }

    private void HandleAnimation()
    {
        int combo = playerData.comboIndex;
        string comboState;

        //melee attack
        if (playerData.isMelee)
        {
            comboState = "PlayerMelee" + combo;
            Debug.Log("Running " + comboState);
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
        
    }

    private void EndCombo()
    {
        Debug.Log("EndCombo");
        bufferCounter = playerData.equippedWeapon.attackBufferList[playerData.comboIndex-1];
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
