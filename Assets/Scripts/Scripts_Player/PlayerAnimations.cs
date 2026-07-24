using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
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
        animationHandler.Play(PlayerAnimMovement.PlayerIdle_SE);
    }

    void Update()
    {
        mouseScreenPos = Mouse.current.position.value;
        mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        mouseDir = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y);

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
                animationHandler.Play(PlayerAnimMovement.PlayerMelee_SE);
            else
                animationHandler.Play(PlayerAnimMovement.PlayerMelee_NW);

            return;
        }

        if (playerMovement.isMoving)
        {
            if (mouseDir.x > 0)
                animationHandler.Play(PlayerAnimMovement.PlayerMove_SE);
            else
                animationHandler.Play(PlayerAnimMovement.PlayerMove_NW);

            return;
        }

        if (mouseDir.x > 0)
            animationHandler.Play(PlayerAnimMovement.PlayerIdle_SE);
        else
            animationHandler.Play(PlayerAnimMovement.PlayerIdle_SW);
    }

    public void Melee(float seconds)
    {
        cooldownMelee += seconds / 5;
    }
}
