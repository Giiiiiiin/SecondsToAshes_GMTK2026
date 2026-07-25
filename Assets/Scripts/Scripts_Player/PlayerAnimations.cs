using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Player_DataHandler playerMovement;
    [SerializeField] private GameObject playerObject; //Aran
    private AnimationHandler animationHandler;

    Vector2 mouseScreenPos;
    Vector3 mousePosition;
    Vector2 mouseDir;

    float cooldownMelee;

    void Awake()
    {
        cooldownMelee = 0;

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
        
        //this should prolly be moved in playermovement and just reference it here
        if (cooldownMelee > 0f)
        {
            cooldownMelee -= Time.deltaTime;
        
            if (cooldownMelee <= 0f)
            {
                cooldownMelee = 0f;
            }
        }
        
        if (playerMovement.isMelee)
        {
            if (mouseDir.x > 0)
                animationHandler.Play(PlayerAnimMovement.PlayerMelee_E);
            else
                animationHandler.Play(PlayerAnimMovement.PlayerMelee_W);
        
            return;
        }
        
        if (playerMovement.isMoving)
        {
            if (mouseDir.x > 0)
                animationHandler.Play(PlayerAnimMovement.PlayerMove_E);
            else
                animationHandler.Play(PlayerAnimMovement.PlayerMove_W);
        
            return;
        }
        
        if (playerMovement.isDodging || playerMovement.isSprinting)
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

    public void Melee(float seconds)
    {
        cooldownMelee += seconds / 5;
    }
}
