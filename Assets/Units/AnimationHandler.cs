using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;
    private string currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // String Call
    public void Play(string animation, float crossfade = 0.1f)
    {
        if (currentState != animation)
        {
            currentState = animation; ;
            animator.CrossFade(animation, crossfade);
        }
    }

    // Enum Call
    public void Play<T>(T animation, float crossfade = 0.1f) where T : System.Enum 
    {
        string animState = animation.ToString();

        if (currentState != animState)
        {
            currentState = animState;
            animator.CrossFade(animState, crossfade);
        }
    }
}
