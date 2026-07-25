using UnityEngine;
using UnityEngine.InputSystem;

public class GunWifeAnimations : MonoBehaviour
{
    [SerializeField] private Player_DataHandler playerMovement;
    private SpriteRenderer sp;

    Vector2 mouseScreenPos;
    Vector3 mousePosition;
    Vector2 mouseDir;

    float cooldownShoot;
    bool default_sprite;

    [Header("Aim")]
    [SerializeField] private Sprite sprite_NE;
    [SerializeField] private Sprite sprite_SE;
    [SerializeField] private Sprite sprite_SW;
    [SerializeField] private Sprite sprite_NW;

    [Header("Attacks")]
    [SerializeField] private Sprite spriteShoot_NE;
    [SerializeField] private Sprite spriteShoot_SE;
    [SerializeField] private Sprite spriteShoot_SW;
    [SerializeField] private Sprite spriteShoot_NW;

    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        sp.sprite = sprite_NE;

        cooldownShoot = 0f;
    }

    void Update()
    {
        mouseScreenPos = Mouse.current.position.value;
        mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        mouseDir = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y);

        if (cooldownShoot > 0f)
        {
            cooldownShoot -= Time.deltaTime;

            if (cooldownShoot <= 0f)
            {
                cooldownShoot = 0f;
                default_sprite = true;
            }
        }

        if (mouseDir.x >= 0 && mouseDir.y >= 0)
        {
            if (default_sprite)
                sp.sprite = sprite_NE;
        }
        else if (mouseDir.x >= 0 && mouseDir.y < 0)
        {
            if (default_sprite)
                sp.sprite = sprite_SE;
        }
        else if (mouseDir.x < 0 && mouseDir.y < 0)
        {
            if (default_sprite)
                sp.sprite = sprite_SW;
        }
        else
        {
            if (default_sprite)
                sp.sprite = sprite_NW;
        }
    }

    public void Shoot(float seconds)
    {
        default_sprite = false;

        if (mouseDir.x >= 0 && mouseDir.y >= 0)
        {
            sp.sprite = spriteShoot_NE;
        }
        else if (mouseDir.x >= 0 && mouseDir.y < 0)
        {
            sp.sprite = spriteShoot_SE;
        }
        else if (mouseDir.x < 0 && mouseDir.y < 0)
        {
            sp.sprite = spriteShoot_SW;
        }
        else
        {
            sp.sprite = spriteShoot_NW;
        }

        cooldownShoot += seconds / 5;
    }
}
