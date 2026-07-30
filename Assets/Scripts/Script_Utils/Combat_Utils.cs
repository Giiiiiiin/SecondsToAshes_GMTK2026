using UnityEngine;

public static class Combat_Utils
{
    public static void FaceDirection(Rigidbody2D rb, Transform t)
    {
        Vector3 localScale = t.transform.localScale;

        if (rb.linearVelocity.x > 0f)
        {
            localScale.x = Mathf.Abs(localScale.x); // right
        }
        else if (rb.linearVelocity.x < 0f)
        {
            localScale.x = -Mathf.Abs(localScale.x); // left
        }
        t.localScale = localScale;
    }
}