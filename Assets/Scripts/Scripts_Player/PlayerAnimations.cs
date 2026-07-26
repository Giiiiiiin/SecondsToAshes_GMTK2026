using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Player_DataHandler playerData;
    [SerializeField] private GameObject playerObject; //Aran

    private AnimationHandler animationHandler;

    void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
    }

    void Update()
    {
        HandleAnimation();
        FaceDirection();
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

    private void FaceDirection()
    {
        Vector3 localScale = transform.localScale;

        if (playerData.rb.linearVelocity.x > 0f)
        {
            localScale.x = Mathf.Abs(localScale.x); // right
        }
        else if (playerData.rb.linearVelocity.x < 0f)
        {
            localScale.x = -Mathf.Abs(localScale.x); // left
        }
        base.transform.localScale = localScale;
    }

    public void EndDodge() 
    {
        playerData.EndDodge();
    }

    public void EndMelee()
    {
        playerData.endMelee();
    }
}
