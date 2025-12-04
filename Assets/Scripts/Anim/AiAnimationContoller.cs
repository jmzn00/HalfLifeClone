using UnityEngine;

public class AiAnimationContoller : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private string lastTrigger = null;
    public void SetTrigger(string t) 
    {
        if (lastTrigger == t)
            return;

        animator.SetTrigger(t);
        lastTrigger = t;
    }
}
