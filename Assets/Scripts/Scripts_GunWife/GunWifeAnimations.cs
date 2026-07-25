using UnityEngine;
using UnityEngine.InputSystem;

public class GunWifeAnimations : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    private AnimationHandler animationHandler;

    Vector2 mouseScreenPos;
    Vector3 mousePosition;
    Vector2 mouseDir;
    
    bool default_sprite;
    
    void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
    }

    void Update()
    {
        mouseScreenPos = Mouse.current.position.value;
        mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        mouseDir = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y);

        if (mouseDir.x >= 0 && mouseDir.y >= 0)
            animationHandler.Play(GunAnimMovement.GunWife_Idle_NE);
        else if (mouseDir.x >= 0 && mouseDir.y < 0)
            animationHandler.Play(GunAnimMovement.GunWife_Idle_SE);
        else if (mouseDir.x < 0 && mouseDir.y < 0)
            animationHandler.Play(GunAnimMovement.GunWife_Idle_SW);
        else if ((mouseDir.x < 0 && mouseDir.y >= 0))
            animationHandler.Play(GunAnimMovement.GunWife_Idle_NW);
    }

    public void Shoot(float seconds)
    {
      //Removed Cooldown logic, it shouldnt be here
    }
}
