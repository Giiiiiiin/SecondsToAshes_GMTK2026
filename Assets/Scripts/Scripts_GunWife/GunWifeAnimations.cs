using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunWifeAnimations : MonoBehaviour
{
    [SerializeField] private Player_DataHandler playerDataHandler;
    private AnimationHandler animationHandler;

    Vector2 mouseScreenPos;
    Vector3 mousePosition;
    Vector2 mouseDir;
    
    bool default_sprite;
    
    void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
    }
    private void OnEnable()
    {
      //  Player_DataHandler.ShootGun.AddListener(Shoot);
    }
    
    private void OnDisable()
    {
       // Player_DataHandler.ShootGun.RemoveListener(Shoot);
    }

    void Update()
    {
        GetMouseDir();
        
        // if (mouseDir.x >= 0 && mouseDir.y >= 0)
        //     animationHandler.Play(GunAnimMovement.GunWife_Idle_NE);
        // else if (mouseDir.x >= 0 && mouseDir.y < 0)
        //     animationHandler.Play(GunAnimMovement.GunWife_Idle_SE);
        // else if (mouseDir.x < 0 && mouseDir.y < 0)
        //     animationHandler.Play(GunAnimMovement.GunWife_Idle_SW);
        // else if ((mouseDir.x < 0 && mouseDir.y >= 0))
        //     animationHandler.Play(GunAnimMovement.GunWife_Idle_NW);
    }
  
    public void Shoot()
    {
        GetMouseDir();
        
        // if (mouseDir.x >= 0 && mouseDir.y >= 0)
        //     animationHandler.Play(GunAnimMovement.GunWife_Attack_NE);
        // else if (mouseDir.x >= 0 && mouseDir.y < 0)
        //     animationHandler.Play(GunAnimMovement.GunWife_Attack_SE);
        // else if (mouseDir.x < 0 && mouseDir.y < 0)
        //     animationHandler.Play(GunAnimMovement.GunWife_Attack_SW);
        // else if ((mouseDir.x < 0 && mouseDir.y >= 0))
        //     animationHandler.Play(GunAnimMovement.GunWife_Attack_NW);
    }
    
    public void GetMouseDir()
    {
        mouseScreenPos = Mouse.current.position.value;
        mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        mouseDir = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y);
    }

    // public void OnEndShootAnim() => playerDataHandler.EndShoot();

}
