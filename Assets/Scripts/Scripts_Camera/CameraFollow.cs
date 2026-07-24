using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    void Start()
    {
        offset = target.position - transform.position;
    }

    void FixedUpdate()
    {
        transform.position = target.position - offset;
    }
}
