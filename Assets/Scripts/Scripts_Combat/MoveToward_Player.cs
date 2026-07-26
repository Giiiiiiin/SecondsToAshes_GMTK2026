using UnityEngine;

public class MoveToward_Player : MonoBehaviour
{
    private PlayerMarker playerMarker;
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float secondsAlive = 5f;

    Vector2 targetDir;

    void Start()
    {
        playerMarker = PlayerMarker.Instance;
        rb = GetComponent<Rigidbody2D>();

        targetDir = new Vector2(
            playerMarker.transform.position.x - transform.position.x,
            playerMarker.transform.position.y - transform.position.y);
    }

    private void Update()
    {
        transform.position += (Vector3)(targetDir.normalized * moveSpeed * Time.deltaTime);

        secondsAlive -= Time.deltaTime;
        if (secondsAlive <= 0)
        {
            Destroy(gameObject);
        }
    }
}
